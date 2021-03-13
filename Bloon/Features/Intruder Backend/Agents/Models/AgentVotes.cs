namespace Bloon.Features.IntruderBackend.Agents
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bloon.Features.IntruderBackend.Votes;
    using Newtonsoft.Json;

    /// <summary>
    /// https://api.intruderfps.com/agents/76561197993774624/votes .
    /// </summary>
    [NotMapped]
    public class AgentVotes
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("agentId")]
        public int AgentID { get; set; }

        [JsonProperty("attributeId")]
        public int AttributeID { get; set; }

        [JsonProperty("positive")]
        public int PositiveVotes { get; set; }

        [JsonProperty("negative")]
        public int NegativeVotes { get; set; }

        [JsonProperty("received")]
        public int ReceivedVotes { get; set; }

        [JsonProperty("lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [JsonProperty("agent")]
#nullable enable
        public string? Agent { get; set; }
#nullable disable

        public VoteAttributes Attribute { get; set; }
    }
}
