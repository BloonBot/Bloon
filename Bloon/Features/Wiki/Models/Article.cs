namespace Bloon.Features.Wiki.Models
{
    public class Article
    {
        public int pageid { get; set; }

        public int ns { get; set; }

        public string title { get; set; }

        public Revision[] revisions { get; set; }

    }
}
