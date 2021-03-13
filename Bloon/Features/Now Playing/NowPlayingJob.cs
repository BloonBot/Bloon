namespace Bloon.Features.NowPlaying
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using Bloon.Variables.Emojis;
    using Bloon.Variables.Roles;
    using DSharpPlus;
    using DSharpPlus.Entities;

    public class NowPlayingJob : ITimedJob
    {
        private readonly DiscordClient dClient;
        private readonly BloonLog bloonLog;

        public NowPlayingJob(DiscordClient dClient, BloonLog bloonLog)
        {
            this.dClient = dClient;
            this.bloonLog = bloonLog;
        }

        public ulong Emoji => SBGEmojis.Superboss;

        public int Interval => 5;

        public async Task Execute()
        {
            DiscordGuild sbg = await this.dClient.GetGuildAsync(Guilds.SBG);
            DiscordRole nowPlayingRole = sbg.GetRole(SBGRoles.NowPlaying);

            List<DiscordMember> prunableMembers = sbg.Members
                .Select(m => m.Value)
                .Where(m => m.Roles.Any(r => r.Id == nowPlayingRole.Id))
                .ToList();

            foreach (DiscordMember member in prunableMembers)
            {
                if (member.Presence == null || !member.Presence.Activities.Any(a => a.Name.Contains("Intruder", StringComparison.Ordinal)))
                {
                    await member.RevokeRoleAsync(nowPlayingRole);
                    this.bloonLog.Information(LogConsole.RoleEdits, ManageRoleEmojis.Demotion, $"**Role Demotion**: {member.Username} - Now Playing");
                }
            }
        }
    }
}
