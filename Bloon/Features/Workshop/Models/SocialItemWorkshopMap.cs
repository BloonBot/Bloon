namespace Bloon.Features.Workshop
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bloon.Core.Services;

    public class SocialItemWorkshopMap : SocialItem
    {
        [Column("type")]
        public override SocialType Type => SocialType.Workshop;

        [NotMapped]
        public WorkshopMapCreator Creator { get; set; }

        [NotMapped]
        public string Description { get; set; }

        [NotMapped]
        public Uri ThumbnailUrl { get; set; }
    }
}
