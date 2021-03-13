namespace Bloon.Features.PackageAccounts
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("agents_history")]
    public class AgentHistory
    {
        [Column("id")]
        [Key]
        public int ID { get; set; }

        [Column("steam_id")]
        public ulong SteamID { get; set; }

        [Column("matches_won")]
        public int MatchesWon { get; set; }

        [Column("matches_lost")]
        public int MatchesLost { get; set; }

        [Column("rounds_lost")]
        public int RoundsLost { get; set; }

        [Column("rounds_tied")]
        public int RoundsTied { get; set; }

        [Column("rounds_won_elims")]
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

        [Column("hacks")]
        public int NetworkHacks { get; set; }

        [Column("survivals")]
        public int Survivals { get; set; }

        [Column("suicides")]
        public int Suicides { get; set; }

        [Column("knockdowns")]
        public int Knockdowns { get; set; }

        [Column("got_knockeddown")]
        public int GotKnockedDown { get; set; }

        [Column("team_knockdowns")]
        public int TeamKnockdowns { get; set; }

        [Column("team_damage")]
        public int TeamDamage { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("login_count")]
        public int LoginCount { get; set; }

        [Column("level_xp")]
        public int LevelXP { get; set; }

        [Column("level")]
        public int Level { get; set; }

        [Column("total_xp")]
        public int TotalXP { get; set; }

        [Column("last_login")]
        public DateTime LastLogin { get; set; }

        [Column("positive_votes")]
        public int PositiveVotes { get; set; }

        [Column("negative_votes")]
        public int NegativeVotes { get; set; }

        [Column("total_votes")]
        public int TotalVotes { get; set; }
    }
}
