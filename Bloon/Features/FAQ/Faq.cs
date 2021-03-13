namespace Bloon.Features.FAQ
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("faq")]
    public class Faq
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("regex")]
        public string Regex { get; set; }

        [Column("message")]
        public string Message { get; set; }
    }
}
