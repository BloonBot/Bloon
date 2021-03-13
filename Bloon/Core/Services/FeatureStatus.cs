namespace Bloon.Core.Services
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("feature_status")]
    public class FeatureStatus
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("enabled")]
        public bool Enabled { get; set; }
    }
}
