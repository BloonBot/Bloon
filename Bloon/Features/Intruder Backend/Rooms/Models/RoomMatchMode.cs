namespace Bloon.Features.IntruderBackend.Rooms
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [NotMapped]
    public class RoomMatchMode
    {
        [Key]
        public int Id { get; set; }

        [JsonProperty("matchModeEnabled")]
        public bool MatchModeEnabled { get; set; }

        [JsonProperty("setsToWin")]
        public int SetsToWin { get; set; }

        [JsonProperty("hatsAllowed")]
        public bool HatsAllowed { get; set; }

        [JsonProperty("timerEnabled")]
        public bool TimerEnabled { get; set; }

        [JsonProperty("limitSpectating")]
        public bool LimitSpectating { get; set; }

        [JsonProperty("reflectiveDamageType")]
        public int ReflectiveDamageType { get; set; }

        [JsonProperty("nextMapAfterXMatches")]
        public int NextMapAfterXMatches { get; set; }

        [JsonProperty("randomMapChange")]
        public bool RandomMapChange { get; set; }
    }
}
