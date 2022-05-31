namespace Bloon.Features.LTP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using Microsoft.Extensions.DependencyInjection;

    public class LTPPruneJob : ITimedJob
    {
        private const int MaxDuration = 7;

        private readonly IServiceScopeFactory scopeFactory;
        private readonly DiscordClient dClient;
        private readonly BloonLog bloonLog;

        public LTPPruneJob(IServiceScopeFactory scopeFactory, DiscordClient dClient, BloonLog bloonLog)
        {
            this.scopeFactory = scopeFactory;
            this.dClient = dClient;
            this.bloonLog = bloonLog;
        }

        public ulong Emoji => Emojis.SBG.Superboss;

        public int Interval => 24 * 60;

        public async Task Execute()
        {
            DiscordGuild sbg = await this.dClient.GetGuildAsync(Guilds.SBG);
            DiscordRole ltpRole = sbg.GetRole(Roles.SBG.LookingToPlay);

            List<DiscordMember> roleMembers = sbg.Members
                .Select(m => m.Value)
                .Where(m => m.Roles.Any(r => r.Id == ltpRole.Id))
                .ToList();
            List<LTPJoin> prunable;

            using IServiceScope scope = this.scopeFactory.CreateScope();
            using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            prunable = db.LTPJoins
                .Where(l => roleMembers.Select(m => m.Id).Contains(l.UserId) && l.Timestamp <= DateTime.UtcNow.AddDays(-MaxDuration))
                .ToList();

            foreach (DiscordMember member in roleMembers)
            {
                if (!prunable.Any(p => p.UserId == member.Id))
                {
                    continue;
                }

                await member.RevokeRoleAsync(ltpRole);

                this.bloonLog.Information(LogConsole.RoleEdits, Emojis.ManageRole.Demotion, $"**Role Demotion**: {member.Username} - LTP");
            }

            db.LTPJoins.RemoveRange(prunable);
            await db.SaveChangesAsync();
        }
    }
}
