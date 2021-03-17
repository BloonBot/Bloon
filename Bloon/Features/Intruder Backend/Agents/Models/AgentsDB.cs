namespace Bloon.Features.IntruderBackend.Agents
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using IntruderLib.Models.Agents;
    using Newtonsoft.Json;

    /// <summary>
    /// Model used to combine the Agents, Agent Stats, and Votes.
    /// </summary>
    [Table("agents")]
    public class AgentsDB
    {
        [Column("steam_id")]
        public ulong SteamID { get; set; }

        [Column("steam_avatar")]
        public string SteamAvatar { get; set; }

        [Key]
        [Column("intruder_id")]
        public int ID { get; set; }

        [Column("role")]
        [JsonProperty("role")]
        public Role Role { get; set; }

        [Column("current_name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Column("old_agent_name")]
        public string OldAgentName { get; set; }

        [Column("matches_won")]
        public int MatchesWon { get; set; }

        [Column("matches_lost")]
        public int MatchesLost { get; set; }

        [Column("rounds_lost")]
        public int RoundsLost { get; set; }

        [Column("rounds_tied")]
        public int RoundsTied { get; set; }

        [Column("rounds_won_elim")]
        public int RoundsWonElim { get; set; }

        [Column("rounds_won_capture")]
        public int RoundsWonCapture { get; set; }

        [Column("rounds_won_hack")]
        public int RoundsWonHack { get; set; }

        [Column("rounds_won_timer")]
        public int RoundsWonTimer { get; set; }

        [Column("rounds_won_custom")]
        public int RoundsWonCustom { get; set; }

        [Column("time_played")]
        public int TimePlayed { get; set; }

        [Column("kills")]
        public int Kills { get; set; }

        [Column("team_kills")]
        public int TeamKills { get; set; }

        [Column("deaths")]
        public int Deaths { get; set; }

        [Column("arrests")]
        public int Arrests { get; set; }

        [Column("got_arrested")]
        public int GotArrested { get; set; }

        [Column("captures")]
        public int Captures { get; set; }

        [Column("pickups")]
        public int Pickups { get; set; }

        [Column("network_hacks")]
        public int NetworkHacks { get; set; }

        [Column("survivals")]
        public int Survivals { get; set; }

        [Column("suicides")]
        public int Suicides { get; set; }

        [Column("knockdowns")]
        public int Knockdowns { get; set; }

        [Column("got_knocked_down")]
        public int GotKnockedDown { get; set; }

        [Column("team_knock_down")]
        public int TeamKnockdowns { get; set; }

        [Column("team_damage")]
        public int TeamDamage { get; set; }

        [Column("level")]
        public int Level { get; set; }

        [Column("level_xp")]
        public int LevelXP { get; set; }

        [Column("level_xp_required")]
        public int? LevelXPRequired { get; set; }

        [Column("total_xp")]
        public int TotalXP { get; set; }

        [Column("positive_votes")]
        public int PositiveVotes { get; set; }

        [Column("negative_votes")]
        public int NegativeVotes { get; set; }

        [Column("received_votes")]
        public int TotalVotes { get; set; }

        [Column("login_count")]
        public int LoginCount { get; set; }

        [Column("first_login")]
        public DateTime FirstLogin { get; set; }

        [Column("last_login")]
        public DateTime LastLogin { get; set; }

        /// <summary>
        /// Gets or sets the timestamp the agent via api.intruderfps.com.
        /// </summary>
        [Column("last_update")]
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Gets or sets The timestamp in which this entry was updated.
        /// </summary>
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
