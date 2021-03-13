namespace Bloon.Features.SBAInactivity
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("sba_inactivity_tracking")]
    public class SBAInactivityTracking
    {
        [Column("user_id")]
        [Key]
        public ulong UserId { get; set; }

        [Column("last_message")]
        public DateTime LastMessage { get; set; }

        [Column("warning_timestamp")]
        public DateTime? WarningTimestamp { get; set; }
    }
}
