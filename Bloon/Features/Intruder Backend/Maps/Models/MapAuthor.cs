namespace Bloon.Features.IntruderBackend.Maps
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [NotMapped]
    public class MapAuthor
    {
        [Key]
        public ulong SteamId { get; set; }

        public string Name { get; set; }

        public string AvatarUrl { get; set; }
    }
}
