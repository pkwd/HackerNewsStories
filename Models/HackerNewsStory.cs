using System.Text.Json.Serialization;
using HackerNewsStories.Converters;

namespace HackerNewsStories.Models
{
    [JsonConverter(typeof(HackerNewsStoryConverter))]
    public class HackerNewsStory
    {
        public string? Title { get; set; }
        public string? Uri { get; set; }      
        public string? PostedBy { get; set; }
        public int Score { get; set; }
        public int CommentCount { get; set; }

        private long _unixTime;

        public string Time => DateTimeOffset
            .FromUnixTimeSeconds(_unixTime)
            .ToUniversalTime()
            .ToString("yyyy-MM-ddTHH:mm:ss+00:00");

        internal void SetUnixTime(long unixTime)
        {
            _unixTime = unixTime;
        }
    }
}

