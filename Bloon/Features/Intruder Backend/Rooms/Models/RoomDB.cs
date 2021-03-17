namespace Bloon.Features.IntruderBackend.Rooms
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using IntruderLib.Models.Rooms;
    using Newtonsoft.Json;

    [Table("room_history")]
    public class RoomDB : Room
    {
        [Key]
        [JsonIgnore]
        public new int Id { get; set; }

        [Column("room_id")]
        [JsonProperty("id")]
        public int RoomId { get; set; }

        [Column("name")]
        [JsonProperty("name")]
        public new string Name { get; set; }

        [Column("region")]
        [JsonProperty("region")]
        public new string Region { get; set; }

        [Column("official")]
        [JsonProperty("official")]
        public new bool Official { get; set; }

        [Column("ranked")]
        [JsonProperty("ranked")]
        public new bool Ranked { get; set; }

        [Column("version")]
        [JsonProperty("version")]
        public new int Version { get; set; }

        [Column("position")]
        [JsonProperty("position")]
        public new int Position { get; set; }

        [Column("agent_count")]
        [JsonProperty("agentCount")]
        public new int AgentCount { get; set; }

        [Column("creator_intruder_id")]
        [JsonProperty("creatorId")]
        public new int CreatorId { get; set; }

        [Column("last_update")]
        [JsonProperty("lastUpdate")]
        public new DateTime LastUpdate { get; set; }

        [Column("current_map")]
        [JsonIgnore]
        public long DBCurrentMap { get; set; }

        [Column("creator_steam_id")]
        [JsonIgnore]
        public ulong DBCreatorSteamId { get; set; }

        [Column("passworded")]
        [JsonIgnore]
        public bool Passworded { get; set; }

        [JsonIgnore]
        [NotMapped]
        public string RegionFlag { get; set; }

        [JsonIgnore]
        [NotMapped]
        public string ServerIcon { get; set; }
    }
}
