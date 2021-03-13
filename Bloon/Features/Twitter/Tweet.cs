namespace Bloon.Features.Twitter
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Bloon.Core.Services;

    public class Tweet : SocialItem
    {
        [Column("type")]
        public override SocialType Type => SocialType.Twitter;
    }
}
