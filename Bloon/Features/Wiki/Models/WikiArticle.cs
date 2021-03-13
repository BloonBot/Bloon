namespace Bloon.Features.Wiki
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Globalization;
    using Bloon.Core.Services;

    [Table("social_wiki_article")]
    public class WikiArticle : SocialItem
    {
        [Column("type")]
        public override SocialType Type => SocialType.Wiki;

        [NotMapped]
        public int PageId
        {
            get
            {
                return this.Additional["pageId"] != null ? int.Parse(this.Additional["pageId"].ToString(), CultureInfo.InvariantCulture) : 0;
            }

            set
            {
                this.Additional["pageId"] = value;
            }
        }

        [NotMapped]
        public string PostType
        {
            get
            {
                return this.Additional["type"]?.ToString() ?? null;
            }

            set
            {
                this.Additional["type"] = value;
            }
        }

        [NotMapped]
        public int ByteDifference
        {
            get
            {
                return this.Additional["byteDifference"] != null ? int.Parse(this.Additional["byteDifference"].ToString(), CultureInfo.InvariantCulture) : 0;
            }

            set
            {
                this.Additional["byteDifference"] = value;
            }
        }

        [NotMapped]
        public int OldPageSize { get; set; }

        [NotMapped]
        public int NewPageSize { get; set; }

        [NotMapped]
        public int RevId { get; set; }

        [NotMapped]
        public int OldRevId { get; set; }

        [NotMapped]
        public string Body { get; set; }

        [NotMapped]
        public Uri Url { get; set; }
    }
}
