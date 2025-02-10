namespace HackerNewsStories.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using HackerNewsStories.Models;

    public class HackerNewsStoryConverter : JsonConverter<HackerNewsStory>
    {
        public override HackerNewsStory Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            var element = document.RootElement;

            var story = new HackerNewsStory();

            if (element.TryGetProperty("title", out var title))
            {
                story.Title = title.GetString();
            }
            if (element.TryGetProperty("url", out var url))
            {
                story.Uri = url.GetString();
            }
            if (element.TryGetProperty("by", out var by))
            {
                story.PostedBy = by.GetString();
            }
            if (element.TryGetProperty("score", out var score))
            {
                story.Score = score.GetInt32();
            }
            if (element.TryGetProperty("descendants", out var descendants))
            {
                story.CommentCount = descendants.GetInt32();
            }
            if (element.TryGetProperty("time", out var time))
            {
                story.SetUnixTime(time.GetInt64());
            }

            return story;
        }

        public override void Write(Utf8JsonWriter writer, HackerNewsStory value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("title", value.Title);
            writer.WriteString("url", value.Uri);
            writer.WriteString("by", value.PostedBy);
            writer.WriteNumber("score", value.Score);
            writer.WriteNumber("descendants", value.CommentCount);
            writer.WriteString("time", value.Time);
            writer.WriteEndObject();
        }
    }
}

