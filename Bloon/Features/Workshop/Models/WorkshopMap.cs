namespace Bloon.Features.Workshop.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("workshop_maps")]
    public class WorkshopMap
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("file_id")]
        [JsonProperty("publishedfileid")]
        public string FileID { get; set; }

        [Column("creator_steam_id")]
        public ulong CreatorSteamID { get; set; }

        [NotMapped]
        [JsonProperty("creator")]
        public string APICreator { get; set; }

        [Column("creator_appid")]
        [JsonProperty("creator_appid")]
        public int CreatorAppID { get; set; }

        [NotMapped]
        [JsonProperty("consumer_appid")]
        public int ConsumerAppID { get; set; }

        [Column("file_size")]
        [JsonProperty("file_size")]
        public string FileSize { get; set; }

        [NotMapped]
        [JsonProperty("preview_url")]
        public string PreviewURL { get; set; }

        [Column("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [Column("short_description")]
        [JsonProperty("short_description")]
        public string Description { get; set; }

        [Column("time_created")]
        public DateTime TimeCreated { get; set; }

        [NotMapped]
        [JsonProperty("time_created")]
        public int UploadDate { get; set; }

        [Column("time_updated")]
        public DateTime TimeUpdated { get; set; }

        [NotMapped]
        [JsonProperty("time_updated")]
        public int MapUpdated { get; set; }

        [Column("visibility")]
        [JsonProperty("visibility")]
        public int Visibility { get; set; }

        [NotMapped]
        [JsonProperty("flags")]
        public int Flags { get; set; }

        [Column("banned")]
        [JsonProperty("banned")]
        public bool Banned { get; set; }

        [Column("ban_reason")]
        [JsonProperty("ban_reason")]
        public string BanReason { get; set; }

        [Column("can_subscribe")]
        [JsonProperty("can_subscribe")]
        public bool CanSubscribe { get; set; }

        [Column("subscriptions")]
        [JsonProperty("subscriptions")]
        public int Subscriptions { get; set; }

        [Column("favorited")]
        [JsonProperty("favorited")]
        public int Favorited { get; set; }

        [Column("followers")]
        [JsonProperty("followers")]
        public int Followers { get; set; }

        [Column("lifetime_subscriptions")]
        [JsonProperty("lifetime_subscriptions")]
        public int LifetimeSubscriptions { get; set; }

        [Column("lifetime_favorited")]
        [JsonProperty("lifetime_favorited")]
        public int LifetimeFavorited { get; set; }

        [Column("lifetime_followers")]
        [JsonProperty("lifetime_followers")]
        public int LifetimeFollowers { get; set; }

        [Column("lifetime_playtime")]
        [JsonProperty("lifetime_playtime")]
        public string LifetimePlaytime { get; set; }

        [Column("lifetime_playtime_sessions")]
        [JsonProperty("lifetime_playtime_sessions")]
        public string LifetimePlaytimeSessions { get; set; }

        [Column("views")]
        [JsonProperty("views")]
        public int Views { get; set; }

        [Column("revision_change_number")]
        [JsonProperty("revision_change_number")]
        public string RevisionChangeNumber { get; set; }

        [Column("revision")]
        [JsonProperty("revision")]
        public int Revision { get; set; }
    }
}
