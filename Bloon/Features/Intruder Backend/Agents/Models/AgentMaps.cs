namespace Bloon.Features.IntruderBackend.Agents
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    /// <summary>
    /// https://api.intruderfps.com/agents/76561198040649261/maps
    /// I don't believe this will actually decode yet so don't try anything funny until its re-worked.
    /// </summary>
    [NotMapped]
    public class AgentMaps
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("creatorId")]
        public long CreatorID { get; set; }

        [JsonProperty("thumbnailUrl")]
        public string ThumbmailURL { get; set; }

        [JsonProperty("favourites")]
        public int Favorites { get; set; }

        [JsonProperty("subscriptions")]
        public int Subscriptions { get; set; }

        [JsonProperty("views")]
        public int Views { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated { get; set; }
    }
}
