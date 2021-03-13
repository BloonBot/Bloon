namespace Bloon.Features.IntruderBackend.Rooms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bloon.Features.Intruder_Backend.Rooms.Models;
    using Bloon.Features.IntruderBackend.Agents;
    using Bloon.Features.IntruderBackend.Maps;
    using Bloon.Variables.Emojis;
    using DSharpPlus.Entities;
    using LinqToTwitter;
    using Newtonsoft.Json;

    [Table("room_history")]
    public class Rooms
    {
        [Key]
        public int Id { get; set; }

        [Column("room_id")]
        [JsonProperty("id")]
        public int RoomId { get; set; }

        [Column("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [NotMapped]
        [JsonProperty("motd")]
        public string Motd { get; set; }

        [NotMapped]
        [JsonProperty("password")]
        public string Password { get; set; }

        [Column("region")]
        [JsonProperty("region")]
        public string Region { get; set; }

        [Column("official")]
        [JsonProperty("official")]
        public bool Official { get; set; }

        [Column("ranked")]
        [JsonProperty("ranked")]
        public bool Ranked { get; set; }

        [NotMapped]
        [JsonProperty("style")]
        public string Style { get; set; }

        [Column("version")]
        [JsonProperty("version")]
        public int Version { get; set; }

        [Column("position")]
        [JsonProperty("position")]
        public int Position { get; set; }

        [NotMapped]
        [JsonProperty("scores")]
        public List<RoomScores> Scores { get; set; }

        [NotMapped]
        public RoomCurrentMap CurrentMap { get; set; }

        [NotMapped]
        public Map[] Maps { get; set; }

        [NotMapped]
        [JsonProperty("maxAgents")]
        public int MaxAgents { get; set; }

        [Column("agent_count")]
        [JsonProperty("agentCount")]
        public int AgentCount { get; set; }

        [NotMapped]
        [JsonProperty("spectatorSlots")]
        public int SpectatorSlots { get; set; }

        [NotMapped]
        [JsonProperty("joinableBy")]
        public string JoinableBy { get; set; }

        [Column("creator_intruder_id")]
        [JsonProperty("creatorId")]
        public int CreatorId { get; set; }

        [NotMapped]
        public RoomRules Rules { get; set; }

        [NotMapped]
        [JsonProperty("tuning")]
        public Object? Tuning { get; set; }

        [Column("last_update")]
        [JsonProperty("lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [NotMapped]
        public Agent Creator { get; set; }

        [Column("current_map")]
        public long DBCurrentMap { get; set; }

        [Column("creator_steam_id")]
        public ulong DBCreatorSteamId { get; set; }

        [Column("passworded")]
        public bool passworded { get; set; }

        [NotMapped]
        public string RegionFlag { get; set; }

        [NotMapped]
        public string ServerIcon { get; set; }

        // statuses of all players in the room? Why tho
        //public AgentStatus statuses { get; set; }
    }
}
