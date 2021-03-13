namespace Bloon.Analytics.Users
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("user_event")]
    public class UserEvent
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public ulong UserId { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("nickname")]
        public string Nickname { get; set; }

        [Column("discriminator")]
        public string Discriminator { get; set; }

        [Column("avatar_id")]
        public string AvatarId { get; set; }

        [Column("account_created")]
        public DateTime AccountCreated { get; set; }

        [Column("is_bot")]
        public bool Bot { get; set; }

        [Column("event")]
        public virtual Event Event { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
