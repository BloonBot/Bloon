namespace Bloon.Core.Database
{
    using Bloon.Analytics.Users;
    using Microsoft.EntityFrameworkCore;

    public partial class AnalyticsContext : DbContext
    {
        public DbSet<UserEvent> UserEvents { get; set; }
    }
}
