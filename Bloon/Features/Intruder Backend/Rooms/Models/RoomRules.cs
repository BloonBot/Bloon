namespace Bloon.Features.IntruderBackend.Rooms
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [NotMapped]
    public class RoomRules
    {
        [Key]
        public int Id { get; set; }

        public RoomMatchMode MatchMode { get; set; }
    }
}
