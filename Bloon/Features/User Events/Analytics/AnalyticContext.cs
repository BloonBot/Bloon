namespace Bloon.Core.Database
{
    using Microsoft.EntityFrameworkCore;
    using Bloon.Analytics.Users;

    public partial class AnalyticsContext : DbContext
    {
        public DbSet<UserEvent> UserEvents { get; set; }
    }
}
