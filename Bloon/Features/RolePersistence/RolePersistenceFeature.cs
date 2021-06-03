namespace Bloon.Features.RolePersistence
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using Bloon.Variables.Channels;
    using Bloon.Variables.Roles;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public class RolePersistenceFeature : Feature
    {
        private static readonly HashSet<ulong> Blacklist = new HashSet<ulong>
        {
            SBGRoles.LookingToPlay,
            SBGRoles.NowPlaying,
            SBGRoles.NowStreaming,
        };

        private readonly IServiceScopeFactory scopeFactory;
        private readonly DiscordClient dClient;

        public RolePersistenceFeature(IServiceScopeFactory scopeFactory, DiscordClient dClient)
        {
            this.scopeFactory = scopeFactory;
            this.dClient = dClient;
        }

        public override string Name => "Role Persistence";

        public override string Description => "Persist roles and reassign when members rejoin.";

        public override Task Disable()
        {
            this.dClient.GuildBanAdded -= this.OnGuildBanAdded;
            this.dClient.GuildMemberAdded -= this.OnGuildMemberAdded;
            this.dClient.GuildMembersChunked -= this.OnGuildMembersChunked;
            this.dClient.GuildMemberUpdated -= this.OnGuildMemberUpdated;
            this.dClient.GuildRoleDeleted -= this.OnGuildRoleDeleted;

            return base.Disable();
        }

        public override Task Enable()
        {
            this.dClient.GuildBanAdded += this.OnGuildBanAdded;
            this.dClient.GuildMemberAdded += this.OnGuildMemberAdded;
            this.dClient.GuildMembersChunked += this.OnGuildMembersChunked;
            this.dClient.GuildMemberUpdated += this.OnGuildMemberUpdated;
            this.dClient.GuildRoleDeleted += this.OnGuildRoleDeleted;

            return base.Enable();
        }

        private static bool ShouldPersist(DiscordGuild guild, DiscordRole role)
        {
            return !role.IsManaged
                && !Blacklist.Contains(role.Id)
                && role.Position < guild.CurrentMember.Hierarchy;
        }

        private async Task OnGuildBanAdded(DiscordClient dClient, GuildBanAddEventArgs args)
        {
            if (args.Guild.Id != Guilds.SBG)
            {
                return;
            }

            IServiceScope scope = this.scopeFactory.CreateScope();
            BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            await db.Database.ExecuteSqlRawAsync("DELETE FROM `role_member` WHERE `id` = {0}", args.Member.Id);
        }

        private async Task OnGuildMemberAdded(DiscordClient dClient, GuildMemberAddEventArgs args)
        {
            if (args.Guild.Id != Guilds.SBG || args.Member.IsBot)
            {
                return;
            }

            IServiceScope scope = this.scopeFactory.CreateScope();
            BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            List<ulong> roleIds = await db.RoleMembers.AsNoTracking()
                .Where(r => r.MemberId == args.Member.Id)
                .Select(r => r.RoleId)
                .ToListAsync();

            List<string> addedRoleNames = new List<string>();

            for (int i = 0; i < roleIds.Count; i++)
            {
                if (args.Guild.Roles.TryGetValue(roleIds[i], out DiscordRole role) && role.Position < args.Guild.CurrentMember.Hierarchy)
                {
                    addedRoleNames.Add(role.Name);
                    await args.Member.GrantRoleAsync(role);
                }
            }

            if (addedRoleNames.Count == 0)
            {
                return;
            }

            await args.Guild.GetChannel(SBGChannels.Bloonside)
                .SendMessageAsync($"Granted **{string.Join(", ", addedRoleNames)}** to **{args.Member.Username}**.");
        }

        private async Task OnGuildMembersChunked(DiscordClient dClient, GuildMembersChunkEventArgs args)
        {
            if (args.Guild.Id != Guilds.SBG)
            {
                return;
            }

            IServiceScope scope = this.scopeFactory.CreateScope();
            BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();

            List<RoleMember> roleMembers = await db.RoleMembers
                .Where(r => args.Members.Select(m => m.Id).Contains(r.MemberId))
                .ToListAsync();

            db.RoleMembers.RemoveRange(roleMembers);

            foreach (DiscordMember member in args.Members)
            {
                IEnumerable<DiscordRole> roles = member.Roles.Where(r => ShouldPersist(args.Guild, r));

                if (!roles.Any() || member.IsBot)
                {
                    continue;
                }

                foreach (DiscordRole role in roles)
                {
                    RoleMember roleMember = roleMembers.Where(r => r.MemberId == member.Id && r.RoleId == role.Id)
                        .FirstOrDefault();

                    if (roleMember != null)
                    {
                        db.Entry(roleMember).State = EntityState.Unchanged;
                    }
                    else
                    {
                        db.RoleMembers.Add(new RoleMember
                        {
                            MemberId = member.Id,
                            RoleId = role.Id,
                        });
                    }
                }
            }

            await db.SaveChangesAsync();
        }

        private async Task OnGuildMemberUpdated(DiscordClient dClient, GuildMemberUpdateEventArgs args)
        {
            if (args.Guild.Id != Guilds.SBG || args.RolesBefore.Count == args.RolesAfter.Count || args.Member.IsBot)
            {
                return;
            }

            bool roleAssigned = args.RolesBefore.Count < args.RolesAfter.Count;
            IEnumerable<DiscordRole> modifiedRoles = roleAssigned
                ? args.RolesAfter.Except(args.RolesBefore)
                : args.RolesBefore.Except(args.RolesAfter);

            IServiceScope scope = this.scopeFactory.CreateScope();
            BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();

            foreach (DiscordRole role in modifiedRoles.Where(m => ShouldPersist(args.Guild, m)))
            {
                RoleMember roleMember = await db.RoleMembers.Where(r => r.MemberId == args.Member.Id && r.RoleId == role.Id)
                    .FirstOrDefaultAsync();

                if (roleMember == null && roleAssigned)
                {
                    db.RoleMembers.Add(new RoleMember
                    {
                        MemberId = args.Member.Id,
                        RoleId = role.Id,
                    });
                }
                else if (roleMember != null && !roleAssigned)
                {
                    db.RoleMembers.Remove(roleMember);
                }
            }

            await db.SaveChangesAsync();
        }

        private async Task OnGuildRoleDeleted(DiscordClient dClient, GuildRoleDeleteEventArgs args)
        {
            if (args.Guild.Id != Guilds.SBG)
            {
                return;
            }

            IServiceScope scope = this.scopeFactory.CreateScope();
            BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            await db.Database.ExecuteSqlRawAsync("DELETE FROM `role_member` WHERE `role_id` = {0}", args.Role.Id);
        }
    }
}
