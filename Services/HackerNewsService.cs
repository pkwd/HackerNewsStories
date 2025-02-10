using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using HackerNewsStories.Models;

namespace HackerNewsStories.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private const string BestStoriesUrl = "https://hacker-news.firebaseio.com/v0/beststories.json";
        private const string ItemUrlTemplate = "https://hacker-news.firebaseio.com/v0/item/{0}.json";
        private const int BestStoryIdsCacheDuration = 5;
        private const int StoryDetailsCacheDuration = 5;

        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<HackerNewsService> _logger;

        private const string BestStoriesCacheKey = "BestStoriesIds";
        private const string StoryCacheKeyPrefix = "Story_";

        public HackerNewsService(HttpClient httpClient, IMemoryCache cache, ILogger<HackerNewsService> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        public async Task<IEnumerable<HackerNewsStory>?> GetBestStoriesAsync(int n)
        {
            var bestStoryIds = await GetBestStoriesIdsAsync();
            
            if(bestStoryIds.Count() < n) 
            {
                n = bestStoryIds.Count(); 
            }

            _logger.LogInformation($"Returning {n} best stories");
            var nBestStoriesIds = bestStoryIds?.Take(n);
            var storiesTasks = nBestStoriesIds?.Select(GetStoryAsync);
            var stories = await Task.WhenAll(storiesTasks);

            var sortedStories = stories
                .Where(s => s != null)
                .OrderByDescending(s => s.Score);

            return sortedStories;
        }

        private async Task<int[]> GetBestStoriesIdsAsync()
        {
            if (!_cache.TryGetValue(BestStoriesCacheKey, out int[]? bestStoryIds))
            {
                _logger.LogInformation("Cache miss for best stories IDs. Fetching from Hacker News...");
                var response = await _httpClient.GetStringAsync(BestStoriesUrl);
                bestStoryIds = JsonSerializer.Deserialize<int[]>(response);

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(BestStoryIdsCacheDuration)
                };
                _cache.Set(BestStoriesCacheKey, bestStoryIds, cacheOptions);
            }
            else
            {
                _logger.LogInformation("Best stories IDs retrieved from cache.");
            }

            return bestStoryIds;
        }

        private async Task<HackerNewsStory?> GetStoryAsync(int storyId)
        {
            var cacheKey = $"{StoryCacheKeyPrefix}{storyId}";
            if (_cache.TryGetValue(cacheKey, out HackerNewsStory? cachedStory))
            {
                return cachedStory;
            }

            try
            {
                var storyUrl = string.Format(ItemUrlTemplate, storyId);
                var response = await _httpClient.GetStringAsync(storyUrl);
                var story = JsonSerializer.Deserialize<HackerNewsStory>(response);

                if (story != null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(StoryDetailsCacheDuration)
                    };
                    _cache.Set(cacheKey, story, cacheOptions);
                }
                return story;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to fetch story with ID = {storyId}");
                return null;
            }
        }
    }
}