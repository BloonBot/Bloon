namespace Bloon.Features.Doorman
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;

    /// <summary>
    /// Logs the events of users joining and leaving into text channels.
    /// </summary>
    public class UserInfoLoggingFeature : Feature
    {
        private readonly DiscordClient dClient;
        private readonly BloonLog bloonLog;

        public UserInfoLoggingFeature(DiscordClient dClient, BloonLog bloonLog)
        {
            this.dClient = dClient;
            this.bloonLog = bloonLog;
        }

        public override string Name => "#sbg_userinfo Logging";

        public override string Description => "Logs Joins/Leaves into #sbg_userinfo";

        public override Task Disable()
        {
            this.dClient.GuildMemberAdded -= this.LogUserJoin;
            this.dClient.GuildMemberRemoved -= this.LogUserLeaveAsync;
            return base.Disable();
        }

        public override Task Enable()
        {
            this.dClient.GuildMemberAdded += this.LogUserJoin;
            this.dClient.GuildMemberRemoved += this.LogUserLeaveAsync;

            return base.Enable();
        }

        private Task LogUserJoin(DiscordClient dClient, GuildMemberAddEventArgs args)
        {
            if (args.Guild.Id != Guilds.SBG)
            {
                return Task.CompletedTask;
            }

            string message = $"{args.Member.Username} **Joined** SBG. Account Created: {args.Member.CreationTimestamp.UtcDateTime.ToString("D", CultureInfo.InvariantCulture)} | ID: {args.Member.Id}";

            this.bloonLog.Information(LogConsole.UserInfo, Emojis.Event.Join, message);

            return Task.CompletedTask;
        }

        private async Task LogUserLeaveAsync(DiscordClient dClient, GuildMemberRemoveEventArgs args)
        {
            if (args.Guild.Id != Guilds.SBG)
            {
                return;
            }

            bool memberCached = true;
            string username;
            string discriminator;

            try
            {
                username = args.Member.Username;
                discriminator = args.Member.Discriminator;
            }
            catch (Exception)
            {
                memberCached = false;
                DiscordUser user = await this.dClient.GetUserAsync(args.Member.Id);
                username = user.Username;
                discriminator = user.Discriminator;
            }

            string message = $"**User Left**: {username}#{discriminator} | ID: {args.Member.Id}";

            if (memberCached)
            {
                message += $" - Joined: {args.Member.JoinedAt.UtcDateTime.ToString("D", CultureInfo.InvariantCulture)}" +
                    $" | [{(DateTime.UtcNow - args.Member.JoinedAt.UtcDateTime).Days} days ago]";
            }
            else
            {
                message += " - Joined: **Unknown**";
            }

            this.bloonLog.Information(LogConsole.UserInfo, Emojis.Event.Leave, message);
        }
    }
}
