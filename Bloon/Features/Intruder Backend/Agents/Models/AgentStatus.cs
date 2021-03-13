namespace Bloon.Features.IntruderBackend.Agents
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    /// <summary>
    /// https://api.intruderfps.com/agents/76561198113257032/ .
    /// </summary>
    [NotMapped]
    public class AgentStatus
    {
        [Key]
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("agentId")]
        public int AgentID { get; set; }

        [JsonProperty("roomId")]
        public int? RoomID { get; set; }

        [JsonProperty("lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [JsonProperty("online")]
        public bool IsOnline { get; set; }

#nullable enable
        public Rooms.Rooms? Room { get; set; }
#nullable disable
    }
}
