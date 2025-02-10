using HackerNewsStories.Models;

namespace HackerNewsStories.Services
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<HackerNewsStory>?> GetBestStoriesAsync(int n);
    }
}