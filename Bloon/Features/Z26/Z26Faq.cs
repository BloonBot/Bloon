namespace Bloon.Features.Z26
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("z26")]
    public class Z26Faq
    {
        [Column("name")]
        [Key]
        public string Name { get; set; }

        [Column("value")]
        public string Value { get; set; }
    }
}
