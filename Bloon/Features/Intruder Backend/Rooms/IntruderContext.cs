namespace Bloon.Core.Database
{
    using Bloon.Features.IntruderBackend.Agents;
    using Bloon.Features.IntruderBackend.Levels;
    using Bloon.Features.IntruderBackend.Maps;
    using Bloon.Features.IntruderBackend.Rooms;
    using Microsoft.EntityFrameworkCore;

    public partial class IntruderContext : DbContext
    {
        public DbSet<Rooms> Rooms { get; set; }
    }
}
