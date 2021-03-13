namespace Bloon.Core.Database
{
    using Bloon.Features.RolePersistence;
    using Microsoft.EntityFrameworkCore;

    public partial class BloonContext : DbContext
    {
        public DbSet<RoleMember> RoleMembers { get; set; }
    }
}
