namespace Bloon.Core.Database
{
    using Bloon.Features.Helprace;
    using Microsoft.EntityFrameworkCore;

    public partial class BloonContext : DbContext
    {
        public DbSet<HelpracePost> HelpracePosts { get; set; }
    }
}
