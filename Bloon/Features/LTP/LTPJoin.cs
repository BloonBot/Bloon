namespace Bloon.Features.LTP
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ltp")]
    public class LTPJoin
    {
        [Column("user_id")]
        [Key]
        public ulong UserId { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
