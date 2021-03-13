namespace Bloon.Features.IntruderBackend.Agents
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    /// <summary>
    /// https://api.intruderfps.com/agents/76561198113257032.
    /// </summary>
    [NotMapped]
    public class Agent
    {
        public AgentStats Stats { get; set; }
        [Key]
        [JsonProperty("id")]
        public int IntruderID { get; set; }

        [JsonProperty("role")]
        public Role Role{ get; set; }

        [JsonProperty("loginCount")]
        public int LoginCount { get; set; }

        [JsonProperty("firstLogin")]
        public DateTime FirstLogin { get; set; }

        [JsonProperty("lastLogin")]
        public DateTime LastLogin { get; set; }

        [JsonProperty("lastUpdate")]
        public DateTime LastUpdate { get; set; }

        public AgentStatus Status { get; set; }

        [JsonProperty("steamId")]
        public ulong SteamID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("avatarUrl")]
        public string AvatarURL { get; set; }
    }
}
