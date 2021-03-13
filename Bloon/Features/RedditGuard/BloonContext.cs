namespace Bloon.Core.Database
{
    using Bloon.Features.RedditGuard;
    using Microsoft.EntityFrameworkCore;

    public partial class BloonContext : DbContext
    {
        public DbSet<RedditPost> RedditPosts { get; set; }
    }
}
