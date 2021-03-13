namespace Bloon.Features.IntruderBackend.Maps
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("maps")]
    public class Map
    {
        [Key]
        public int Id { get; set; }

        [Column("intruder_id")]
        [JsonProperty("id")]
        public long IntruderId { get; set; }

        [NotMapped]
        public MapAuthor Author { get; set; }

        [Column("author_steam_id")]
        public long DBAuthorSteamID { get; set; }

        [Column("author_name")]
        public long DBAuthorName { get; set; }

        [Column("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [NotMapped]
        [JsonProperty("gamemode")]
        public string Gamemode { get; set; }

        [Column("thumbnail_url")]
        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl { get; set; }

        [Column("is_mapmaker_map")]
        [JsonProperty("isMapMakerMap")]
        public bool IsMapMakerMap { get; set; }

        [NotMapped]
        public List<string> Tags { get; set; }

        [Column("last_update")]
        [JsonProperty("lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [Column("play_count")]
        public int PlayCount { get; set; }
    }
}
