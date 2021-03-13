namespace Bloon.Features.Helprace
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Bloon.Core.Services;

    public class HelpracePost : SocialItem
    {
        [Column("type")]
        public override SocialType Type => SocialType.Helprace;

        [NotMapped]
        public string Body { get; set; }

        [NotMapped]
        public string Channel
        {
            get
            {
                return this.Additional["channel"]?.ToString() ?? null;
            }

            set
            {
                this.Additional["channel"] = value;
            }
        }
    }
}
