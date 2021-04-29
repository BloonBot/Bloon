namespace Bloon.Analytics
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("commands")]
    public class Commands
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("command")]
        public string Command { get; set; }

        [Column("guild")]
        public ulong Guild { get; set; }

        [Column("channel")]
        public ulong Channel { get; set; }

        [Column("user_id")]
        public ulong UserId { get; set; }

        [Column("link")]
        public string Link { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
