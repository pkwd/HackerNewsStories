using System.Text.Json.Serialization;

namespace HackerNewsStories.Models
{
    public class HackerNewsStory
    {
        // Private backing field for the Unix timestamp
        [JsonInclude]
        [JsonPropertyName("time")]
        private long _unixTime;

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("url")]
        public string? Uri { get; set; }

        [JsonPropertyName("by")]
        public string? PostedBy { get; set; }

        public string Times => DateTimeOffset
           .FromUnixTimeSeconds(_unixTime)
           .ToUniversalTime()
           .ToString("yyyy-MM-ddTHH:mm:ss+00:00");

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("descendants")]
        public int CommentCount { get; set; }
    }
}

