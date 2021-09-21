namespace Bloon.Features.IntruderBackend.Agents
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bloon.Features.IntruderBackend.Agents;

    [Table("intruder_db_agents")]
    public class IntruderDBAgent
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("steam_id")]
        public ulong SteamID { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("role")]
        public IntruderDBAgentRole Role { get; set; }

        [Column("xp")]
        public int XP { get; set; }

        [Column("time_played")]
        public int TimePlayed { get; set; }

        [Column("matches_played")]
        public int MatchesPlayed { get; set; }

        [Column("matches_won")]
        public int MatchesWon { get; set; }

        [Column("matches_lost")]
        public int MatchesLost { get; set; }

        [Column("matches_survived")]
        public int MatchesSurvived { get; set; }

        [Column("arrests")]
        public int Arrests { get; set; }

        [Column("captures")]
        public int Captures { get; set; }

        [Column("last_update")]
        public DateTime LastUpdate { get; set; }

        [Column("last_seen")]
        public DateTime LastSeen { get; set; }

        [Column("registered")]
        public DateTime Registered { get; set; }
    }
}
