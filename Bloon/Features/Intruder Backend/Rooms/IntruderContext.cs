namespace Bloon.Core.Database
{
    using Bloon.Features.IntruderBackend.Rooms;
    using Microsoft.EntityFrameworkCore;

    public partial class IntruderContext : DbContext
    {
        public DbSet<Rooms> Rooms { get; set; }
    }
}
