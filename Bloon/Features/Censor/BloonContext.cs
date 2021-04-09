namespace Bloon.Core.Database
{
    using Bloon.Features.Censor;
    using Microsoft.EntityFrameworkCore;

    public partial class BloonContext : DbContext
    {
        public DbSet<Censor> Censors { get; set; }
    }
}
