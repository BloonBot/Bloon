namespace Bloon.Features.Wiki
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("wiki_user")]
    public class WikiUser
    {
        [Column("id")]
        [JsonProperty("userid")]
        public int Id { get; set; }

        [Column("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Column("edits")]
        public int CountEdits { get; set; }

        [NotMapped]
        [JsonProperty("recenteditcount")]
        public int CountRecentPosts { get; set; }
    }
}
