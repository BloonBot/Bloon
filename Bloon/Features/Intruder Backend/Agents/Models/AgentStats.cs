namespace Bloon.Features.IntruderBackend.Agents
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    /// <summary>
    /// https://api.intruderfps.com/agents/76561198113257032/stats .
    /// </summary>
    ///
    [NotMapped]
    public class AgentStats
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("agentId")]
        public int AgentID { get; set; }

        [JsonProperty("matchesWon")]
        public int MatchesWon { get; set; }

        [JsonProperty("matchesLost")]
        public int MatchesLost { get; set; }

        [JsonProperty("roundsLost")]
        public int RoundsLost { get; set; }

        [JsonProperty("roundsTied")]
        public int RoundsTied { get; set; }

        [JsonProperty("roundsWonElimination")]
        public int RoundsWonElim { get; set; }

        [JsonProperty("roundsWonCapture")]
        public int RoundsWonCapture { get; set; }

        [JsonProperty("roundsWonHack")]
        public int RoundsWonHack { get; set; }

        [JsonProperty("roundsWonTimer")]
        public int RoundsWonTimer { get; set; }

        [JsonProperty("roundsWonCustom")]
        public int RoundsWonCustom { get; set; }

        [JsonProperty("timePlayed")]
        public int TimePlayed { get; set; }

        [JsonProperty("kills")]
        public int Kills { get; set; }

        [JsonProperty("teamKills")]
        public int TeamKills { get; set; }

        [JsonProperty("deaths")]
        public int Deaths { get; set; }

        [JsonProperty("arrests")]
        public int Arrests { get; set; }

        [JsonProperty("gotArrested")]
        public int GotArrested { get; set; }

        [JsonProperty("captures")]
        public int Captures { get; set; }

        [JsonProperty("pickups")]
        public int Pickups { get; set; }

        [JsonProperty("networkHacks")]
        public int NetworkHacks { get; set; }

        [JsonProperty("survivals")]
        public int Survivals { get; set; }

        [JsonProperty("suicides")]
        public int Suicides { get; set; }

        [JsonProperty("knockdowns")]
        public int Knockdowns { get; set; }

        [JsonProperty("gotKnockedDown")]
        public int GotKnockedDown { get; set; }

        [JsonProperty("teamKnockdowns")]
        public int TeamKnockdowns { get; set; }

        [JsonProperty("teamDamage")]
        public int TeamDamage { get; set; }

        [JsonProperty("lastUpdate")]
        public DateTime LastUpdate { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("levelXp")]
        public int LevelXP { get; set; }

        [JsonProperty("levelXpRequired")]
        public int? LevelXPRequired { get; set; }

        [JsonProperty("totalXp")]
        public int TotalXP { get; set; }
    }
}
