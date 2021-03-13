namespace Bloon.Features.IntruderBackend.Rooms
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [NotMapped]
    public class RoomObject
    {
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        public Rooms[] Data { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("perPage")]
        public int EntriesPerPage { get; set; }
    }
}
