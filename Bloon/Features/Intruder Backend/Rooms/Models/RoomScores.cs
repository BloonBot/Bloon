namespace Bloon.Features.IntruderBackend.Rooms.Models
{
    using Newtonsoft.Json;

    public class RoomScores
    {
        [JsonProperty("guards")]
        public int Guards { get; set; }

        [JsonProperty("intruders")]
        public int Intruders { get; set; }
    }
}
