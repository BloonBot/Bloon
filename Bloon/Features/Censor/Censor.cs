namespace Bloon.Features.Censor
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("censor")]
    public class Censor
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("pattern")]
        public string Pattern { get; set; }
    }
}
