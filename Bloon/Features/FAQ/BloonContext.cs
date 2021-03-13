namespace Bloon.Core.Database
{
    using Bloon.Features.FAQ;
    using Microsoft.EntityFrameworkCore;

    public partial class BloonContext : DbContext
    {
        public DbSet<Faq> Faqs { get; set; }
    }
}
