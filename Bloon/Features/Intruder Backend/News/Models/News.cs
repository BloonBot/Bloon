namespace Bloon.Features.IntruderBackend.News
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// https://api.intruderfps.com/news .
    /// </summary>
    public class News
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("uid")]
        public string UID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("linkText")]
        public string LinkText { get; set; }

        [JsonProperty("linkUrl")]
        public string LinkURL { get; set; }

        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty("live")]
        public bool Live { get; set; }

        [JsonProperty("sticky")]
        public bool Sticky { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}
