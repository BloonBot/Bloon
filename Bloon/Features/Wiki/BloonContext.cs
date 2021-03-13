namespace Bloon.Core.Database
{
    using Bloon.Features.Wiki;
    using Microsoft.EntityFrameworkCore;

    public partial class BloonContext : DbContext
    {
        public DbSet<WikiArticle> WikiArticles { get; set; }
    }
}
