namespace Bloon.Core.Database
{
    using Bloon.Features.Z26;
    using Microsoft.EntityFrameworkCore;

    public partial class BloonContext : DbContext
    {
        public DbSet<Z26Faq> Z26Faqs { get; set; }
    }
}
