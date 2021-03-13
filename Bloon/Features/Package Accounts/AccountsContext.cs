namespace Bloon.Core.Database
{
    using Bloon.Features.PackageAccounts;
    using Microsoft.EntityFrameworkCore;

    public partial class AccountsContext : DbContext
    {
        public DbSet<PackageAccount> Accounts { get; set; }
    }
}
