namespace Bloon.Features.Wiki.Models
{
    using System;
    using Newtonsoft.Json;

    public class RecentChange
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("ns")]
        public int Ns { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("pageid")]
        public int PageId { get; set; }

        [JsonProperty("revid")]
        public int RevId { get; set; }

        [JsonProperty("old_revid")]
        public int OldRevId { get; set; }

        [JsonProperty("rcid")]
        public int RcId { get; set; }

        [JsonProperty("user")]
        public string User { get; set; }

        [JsonProperty("minor")]
        public string Minor { get; set; }

        [JsonProperty("oldlen")]
        public int OldLen { get; set; }

        [JsonProperty("newlen")]
        public int NewLen { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
