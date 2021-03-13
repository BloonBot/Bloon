namespace Bloon.Core.Database
{
    using Bloon.Features.Youtube;
    using Microsoft.EntityFrameworkCore;

    public partial class BloonContext : DbContext
    {
        public DbSet<YouTubeVideo> YoutubeVideos { get; set; }
    }
}
