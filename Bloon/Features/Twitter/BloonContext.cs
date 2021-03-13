namespace Bloon.Core.Database
{
    using Bloon.Features.Twitter;
    using Microsoft.EntityFrameworkCore;

    public partial class BloonContext : DbContext
    {
        public DbSet<Tweet> Tweets { get; set; }
    }
}
