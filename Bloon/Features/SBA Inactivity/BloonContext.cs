namespace Bloon.Core.Database
{
    using Bloon.Features.SBAInactivity;
    using Microsoft.EntityFrameworkCore;

    public partial class BloonContext : DbContext
    {
        public DbSet<SBAInactivityTracking> SBAInactivityTracking { get; set; }
    }
}
