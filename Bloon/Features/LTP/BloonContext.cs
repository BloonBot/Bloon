namespace Bloon.Core.Database
{
    using Bloon.Features.LTP;
    using Microsoft.EntityFrameworkCore;

    public partial class BloonContext : DbContext
    {
        public DbSet<LTPJoin> LTPJoins { get; set; }
    }
}
