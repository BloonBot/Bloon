namespace Bloon.Core.Database
{
    using Bloon.Features.SteamNews;
    using Microsoft.EntityFrameworkCore;

    public partial class BloonContext : DbContext
    {
        public DbSet<SteamNewsPost> SteamNewsPosts { get; set; }
    }
}
