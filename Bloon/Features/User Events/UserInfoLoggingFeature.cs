namespace Bloon.Features.Doorman
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Features.Analytics;
    using Bloon.Variables.Emojis;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;

    /// <summary>
    /// Logs the events of users joining, leaving, and banning into the database and text channels.
    /// </summary>
    public class UserInfoLoggingFeature : Feature
    {
        private readonly DiscordClient dClient;
        private readonly BloonLog bloonLog;
        private readonly UserEventService userEventService;

        public UserInfoLoggingFeature(DiscordClient dClient, BloonLog bloonLog, UserEventService userEventService)
        {
            this.dClient = dClient;
            this.bloonLog = bloonLog;
            this.userEventService = userEventService;
        }

        public override string Name => "#sbg_userinfo Logging";

        public override string Description => "Logs Joins/Leaves/Bans into DB/Discord Chats";

        public override Task Disable()
        {
            this.dClient.GuildMemberAdded -= this.DBLogMemberJoined;
            this.dClient.GuildMemberAdded += this.DiscordLogUserJoin;

            this.dClient.GuildMemberRemoved -= this.DBLogMemberLeft;
            this.dClient.GuildMemberRemoved -= this.DiscordLogUserLeft;

            this.dClient.GuildBanAdded -= this.DBLogBanAdded;
            this.dClient.GuildBanRemoved -= this.DBLogBanRemoved;
            return base.Disable();
        }

        public override Task Enable()
        {
            this.dClient.GuildMemberAdded += this.DBLogMemberJoined;
            this.dClient.GuildMemberAdded += this.DiscordLogUserJoin;

            this.dClient.GuildMemberRemoved += this.DBLogMemberLeft;
            this.dClient.GuildMemberRemoved += this.DiscordLogUserLeft;

            this.dClient.GuildBanAdded += this.DBLogBanAdded;
            this.dClient.GuildBanRemoved += this.DBLogBanRemoved;

            return base.Enable();
        }

        private static bool IsSBG(ulong guildId) => guildId == Variables.Guilds.SBG;

        private async Task DBLogBanRemoved(DiscordClient sender, GuildBanRemoveEventArgs args)
        {
            if (IsSBG(args.Guild.Id))
            {
                await this.userEventService.AddUserUnBannedEventAsync(args);
            }
        }

        private async Task DBLogBanAdded(DiscordClient sender, GuildBanAddEventArgs args)
        {
            if (IsSBG(args.Guild.Id))
            {
                await this.userEventService.AddUserBannedEventAsync(args);
            }
        }

        private async Task DBLogMemberJoined(DiscordClient dClient, GuildMemberAddEventArgs args)
        {
            if (IsSBG(args.Guild.Id))
            {
                await this.userEventService.AddUserJoinedEventAsync(args);
            }

            // Log to Discord
            this.bloonLog.Information(LogConsole.UserInfo, EventEmojis.Join, $"{args.Member.Username} **Joined** SBG. Account Created: {args.Member.CreationTimestamp.UtcDateTime.ToString("D", CultureInfo.InvariantCulture)} | ID: {args.Member.Id}");
        }

        private async Task DBLogMemberLeft(DiscordClient dClient, GuildMemberRemoveEventArgs args)
        {
            if (IsSBG(args.Guild.Id))
            {
                await this.userEventService.AddUserLeftEventAsync(args);
            }
        }

        private Task DiscordLogUserJoin(DiscordClient dClient, GuildMemberAddEventArgs args)
        {
            if (args.Guild.Id != Variables.Guilds.SBG)
            {
                return Task.CompletedTask;
            }

            this.bloonLog.Information(LogConsole.UserInfo, EventEmojis.Join, $"{args.Member.Username} **Joined** SBG. Account Created: {args.Member.CreationTimestamp.UtcDateTime.ToString("D", CultureInfo.InvariantCulture)} | ID: {args.Member.Id}");

            return Task.CompletedTask;
        }

        private async Task DiscordLogUserLeft(DiscordClient dClient, GuildMemberRemoveEventArgs args)
        {
            if (args.Guild.Id != Variables.Guilds.SBG)
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

            this.bloonLog.Information(LogConsole.UserInfo, EventEmojis.Leave, message);
        }
    }
}
