namespace Bloon.Features.Youtube
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Bloon.Core.Services;

    [Table("social_youtube_video")]
    public class YouTubeVideo : SocialItem
    {
        [Column("type")]
        public override SocialType Type => SocialType.YouTube;

        [NotMapped]
        public string Description { get; set; }

        [NotMapped]
#pragma warning disable CA1056 // Uri properties should not be strings
        public string ThumbnailUrl { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings
    }
}
