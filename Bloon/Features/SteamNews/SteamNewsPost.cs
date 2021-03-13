namespace Bloon.Features.SteamNews
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bloon.Core.Services;

    public class SteamNewsPost : SocialItem
    {
        [Column("type")]
        public override SocialType Type => SocialType.SteamNews;

        [NotMapped]
        public string Description { get; set; }

        [NotMapped]
        public Uri ImageUrl { get; set; }

        [NotMapped]
        public Uri Url { get; set; }
    }
}
