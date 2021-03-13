namespace Bloon.Features.SBAInactivity
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using Bloon.Variables.Channels;
    using Bloon.Variables.Emojis;
    using Bloon.Variables.Roles;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    public class SBAInactivityJob : ITimedJob
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly DiscordClient dClient;
        private readonly BloonLog bloonLog;

        public SBAInactivityJob(IServiceScopeFactory scopeFactory, DiscordClient dClient, BloonLog bloonLog)
        {
            this.scopeFactory = scopeFactory;
            this.dClient = dClient;
            this.bloonLog = bloonLog;
        }

        public ulong Emoji => SBGEmojis.Superboss;

        public int Interval => 24 * 60;

        public async Task Execute()
        {
            DiscordGuild sbg = await this.dClient.GetGuildAsync(Guilds.SBG);
            DiscordRole sbaRole = sbg.GetRole(SBGRoles.SBA);
            DiscordChannel sbaChannel = await this.dClient.GetChannelAsync(SBGChannels.SecretBaseAlpha);

            List<DiscordMember> roleMembers = sbg.Members
                .Select(m => m.Value)
                .Where(m => m.Roles.Any(r => r.Id == sbaRole.Id))
                .ToList();
            Dictionary<ulong, SBAInactivityTracking> tracked;

            using IServiceScope scope = this.scopeFactory.CreateScope();
            using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            tracked = db.SBAInactivityTracking
                .Where(s => roleMembers.Select(m => m.Id).Contains(s.UserId))
                .ToDictionary(x => x.UserId, x => x);

            // Check each user within the guild with the SBA role. Wonder if this cares if the user is online or not?
            foreach (DiscordMember user in roleMembers)
            {
                if (!tracked.ContainsKey(user.Id))
                {
                    this.bloonLog.Information(LogConsole.RoleEdits, ManageRoleEmojis.Warning, $"**Inactivity Error**: No stored message for {user.Username} - SBA");
                    continue;
                }

                SBAInactivityTracking trackedUser = tracked[user.Id];

                double daysInactive = (DateTime.UtcNow - trackedUser.LastMessage).TotalDays;

                if (daysInactive < 14)
                {
                    continue;
                }

                double daysSinceLastWarning = trackedUser.WarningTimestamp.HasValue ? (DateTime.UtcNow - trackedUser.WarningTimestamp.Value).TotalDays : -1;

                // Last chance
                if (daysSinceLastWarning == -1 || daysSinceLastWarning >= 14)
                {
                    string dmMessage = "Hello agent,\n"
                        + "It seems you haven't been active in the SuperbossGames Discord for over 2 weeks.\n"
                        + "The 'Secret Base Alpha' role and access levels will be removed if you remain inactive!\n"
                        + $"Last activity: {trackedUser.LastMessage.ToString("D", CultureInfo.InvariantCulture)}";

                    await this.SendDM(user, dmMessage, false);
                    this.bloonLog.Information(LogConsole.RoleEdits, ManageRoleEmojis.Warning, $"**Inactivity Warning**: {user.Username} - SBA");

                    trackedUser.WarningTimestamp = DateTime.Now;
                    db.Update(trackedUser);
                }

                // Too late
                else if (daysInactive >= 21 && daysSinceLastWarning >= 7)
                {
                    await user.RevokeRoleAsync(sbaRole);

                    string dmMessage = "Hello again,\n"
                        + "Your 'Secret Base Alpha' role and access levels have been removed due to inactivity!\n"
                        + "I'm truly sorry, but I have my orders.\n\n"
                        + "P.S. I did warn you";
                    await this.SendDM(user, dmMessage, true);

                    db.Remove(trackedUser);

                    await sbaChannel.SendMessageAsync($"Kicked {user.Username}#{user.Discriminator} out of SBA");
                    this.bloonLog.Information(LogConsole.RoleEdits, ManageRoleEmojis.Demotion, $"**Inactivity Role Demotion**: {user.Username} - SBA");
                }

                await db.SaveChangesAsync();
            }
        }

        private async Task SendDM(DiscordMember member, string message, bool kicked)
        {
            DiscordDmChannel dmChannel = await member.CreateDmChannelAsync();
            DiscordChannel sbaChannel = await this.dClient.GetChannelAsync(SBGChannels.SecretBaseAlpha);

            try
            {
                await dmChannel.SendMessageAsync(message);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                // Send the message in SBA as a public message to warn the user before we kick them.
                if (!kicked)
                {
                    await sbaChannel.SendMessageAsync($"{member.Mention}, we tried to send you a message but failed. {message}");
                }

                Log.Error(e, $"Unable to send an inactivity DM to {member.Username} : {member.Id}");
                this.bloonLog.Error($"Unable to send an inactivity DM to {member.Username} : {member.Id}");
            }
        }
    }
}
