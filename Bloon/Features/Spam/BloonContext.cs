namespace Bloon.Core.Database
{
    using Bloon.Features.Spam;
    using Microsoft.EntityFrameworkCore;

    public partial class BloonContext : DbContext
    {
        public DbSet<SpamMessage> SpamMessages { get; set; }
    }
}
