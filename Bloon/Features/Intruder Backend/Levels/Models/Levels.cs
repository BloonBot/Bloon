namespace Bloon.Features.IntruderBackend.Levels
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("level")]
    public class Levels
    {
        [Column("level")]
        [Key]
        public int Level { get; set; }

        [Column("xp_required")]
        public int? XPRequired { get; set; }

    }
}
