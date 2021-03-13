using Newtonsoft.Json;
using System;

namespace Bloon.Features.Wiki.Models
{
    public class RecentChanges
    {
        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("ns")]
        public int ns { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("pageid")]
        public int pageid { get; set; }

        [JsonProperty("revid")]
        public int revid { get; set; }

        [JsonProperty("old_revid")]
        public int old_revid { get; set; }

        [JsonProperty("rcid")]
        public int rcid { get; set; }

        [JsonProperty("user")]
        public string user { get; set; }

        [JsonProperty("minor")]
        public string minor { get; set; }

        [JsonProperty("oldlen")]
        public int oldlen { get; set; }

        [JsonProperty("newlen")]
        public int newlen { get; set; }

        [JsonProperty("timestamp")]
        public DateTime timestamp { get; set; }
    }
}
