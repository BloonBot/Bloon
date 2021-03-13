namespace Bloon.Features.RolePersistence
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("role_member")]
    public class RoleMember
    {
        [Column("id")]
        public uint Id { get; set; }

        [Column("member_id")]
        public ulong MemberId { get; set; }

        [Column("role_id")]
        public ulong RoleId { get; set; }
    }
}
