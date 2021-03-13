namespace Bloon.Core.Services
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json.Linq;

    [Table("social_item")]
    public abstract class SocialItem
    {
        public SocialItem()
        {
            this.Additional = new JObject();
        }

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("uid")]
        public string UID { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("author")]
        public string Author { get; set; }

        [Column("type")]
        public virtual SocialType Type { get; set; }

        [Column("additional")]
#pragma warning disable CA2227 // Collection properties should be read only
        public JObject Additional { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
