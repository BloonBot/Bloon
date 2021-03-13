namespace Bloon.Features.Wiki.Models
{
    public class Parse
    {
        public string title { get; set; }
        public int pageid { get; set; }
        public int revid { get; set; }
        public object[] redirects { get; set; }
        public Wikitext wikitext { get; set; }
    }
}
