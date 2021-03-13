namespace Bloon.Features.Spam
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("message")]
    public class SpamMessage
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("message")]
        public string Value { get; set; }
    }
}
