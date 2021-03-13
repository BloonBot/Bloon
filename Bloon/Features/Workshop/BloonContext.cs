namespace Bloon.Core.Database
{
    using Bloon.Features.Workshop;
    using Microsoft.EntityFrameworkCore;

    public partial class BloonContext : DbContext
    {
        public DbSet<SocialItemWorkshopMap> SocialItemWorkshopMap { get; set; }
    }
}
