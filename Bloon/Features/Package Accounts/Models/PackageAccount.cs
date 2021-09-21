namespace Bloon.Features.PackageAccounts
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("package_account")]
    public class PackageAccount
    {
        [Key]
        [Column("steam_id")]
        public ulong SteamID { get; set; }

        [Column("discord_id")]
        public ulong DiscordID { get; set; }

        [Column("pin")]
        public int Pin { get; set; }

        [Column("permissions")]
        public virtual Permission Type { get; set; }

        [Column("tier")]
        public virtual Permission Tier { get; set; }

        [Column("private_profile")]
        public bool PrivateProfile { get; set; }

        [Column("package_created")]
        public DateTime AccountCreated { get; set; }

        [Column("last_login")]
        public DateTime LastLogin { get; set; }

        [Column("steam_profile_url")]
        public string ProfileURL { get; set; }
    }
}
