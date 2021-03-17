namespace Bloon.Core.Database
{
    using Bloon.Features.IntruderBackend.Rooms;
    using Microsoft.EntityFrameworkCore;

    public partial class IntruderContext : DbContext
    {
        public DbSet<RoomDB> Rooms { get; set; }
    }
}
