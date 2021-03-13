namespace Bloon.Features.Wiki.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Query
    {
        [JsonProperty("recentchanges")]
        public List<RecentChange> RecentChanges { get; set; }
    }
}
