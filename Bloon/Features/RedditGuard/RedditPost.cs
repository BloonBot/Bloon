namespace Bloon.Features.RedditGuard
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Bloon.Core.Services;

    public class RedditPost : SocialItem
    {
        [Column("type")]
        public override SocialType Type => SocialType.Reddit;
    }
}
