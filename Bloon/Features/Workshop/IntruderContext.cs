namespace Bloon.Core.Database
{
    using Bloon.Features.IntruderBackend.Agents;
    using Bloon.Features.Workshop.Models;
    using Microsoft.EntityFrameworkCore;

    public partial class IntruderContext : DbContext
    {
        public DbSet<WorkshopMap> WorkshopMaps { get; set; }
    }
}
