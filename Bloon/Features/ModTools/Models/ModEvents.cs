namespace Bloon.Features.ModTools
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mod_events")]
    public class ModEvents
    {
        [Column("id")]
        [Key]
        public int ID { get; set; }

        [Column("mod_id")]
        public ulong ModID { get; set; }

        [Column("event")]
        public virtual Actions Event { get; set; }

        [Column("offender_id")]
        public ulong OffenderID { get; set; }

        [Column("notes")]
        public string Notes { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
