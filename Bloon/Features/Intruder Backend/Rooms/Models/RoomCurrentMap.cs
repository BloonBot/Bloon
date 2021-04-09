namespace Bloon.Features.IntruderBackend.Rooms
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bloon.Features.IntruderBackend.Maps;
    using Newtonsoft.Json;

    [NotMapped]
    public class RoomCurrentMap
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        public MapAuthor Author { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("gamemode")]
        public string Gamemode { get; set; }

        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty("isMapMakerMap")]
        public bool IsMapMakerMap { get; set; }

        [NotMapped]
        public List<string> Tags { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
