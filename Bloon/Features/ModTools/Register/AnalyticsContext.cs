namespace Bloon.Core.Database
{
    using Bloon.Features.ModTools;
    using Microsoft.EntityFrameworkCore;

    public partial class AnalyticsContext : DbContext
    {
        public DbSet<ModEvent> ModEvents { get; set; }
    }
}
