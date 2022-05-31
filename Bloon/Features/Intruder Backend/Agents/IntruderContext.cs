namespace Bloon.Core.Database
{
    using Bloon.Features.IntruderBackend.Agents;
    using Bloon.Features.IntruderBackend.Levels;
    using Microsoft.EntityFrameworkCore;

    public partial class IntruderContext : DbContext
    {
        public DbSet<AgentsDB> Agents { get; set; }

        public DbSet<Levels> Levels { get; set; }
    }
}
