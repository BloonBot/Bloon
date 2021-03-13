namespace Bloon.Features.Doorman
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables.Channels;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;

    public class GoodbyeGreeting : Feature
    {
        private readonly DiscordClient dClient;
        private readonly BloonLog bloonLog;

        public GoodbyeGreeting(DiscordClient dClient, BloonLog bloonLog)
        {
            this.dClient = dClient;
            this.bloonLog = bloonLog;
        }

        public override string Name => "Goodbye Messages";

        public override string Description => "Responds with a 'Goodbye' message whenever a user leaves the SuperbossGames guild.";

        public override Task Disable()
        {
            this.dClient.GuildMemberRemoved -= this.OnGuildMemberRemovedAsync;

            return base.Disable();
        }

        public override Task Enable()
        {
            this.dClient.GuildMemberRemoved += this.OnGuildMemberRemovedAsync;

            return base.Enable();
        }

        private async Task OnGuildMemberRemovedAsync(DiscordClient dClient, GuildMemberRemoveEventArgs args)
        {
            if (args.Guild.Id != Variables.Guilds.SBG)
            {
                return;
            }

            DiscordChannel sbgAug = await this.dClient.GetChannelAsync(SBGChannels.Bloonside).ConfigureAwait(false);

            string departure;
            Random random = new Random();
            int randomValue = random.Next(0, 2);

            // 50:50 chance
            if (randomValue == 0)
            {
                departure = "Goodbye";
            }
            else
            {
                departure = "Goobye";
            }

            // Temp fix for https://github.com/DSharpPlus/DSharpPlus/pull/491
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
                DiscordUser user = await this.dClient.GetUserAsync(args.Member.Id)
                    .ConfigureAwait(false);
                username = user.Username;
                discriminator = user.Discriminator;
            }

            string message = $"**User Left**: {username}#{discriminator}";

            if (memberCached)
            {
                message += $" - Joined: {args.Member.JoinedAt.UtcDateTime.ToString("D", CultureInfo.InvariantCulture)}" +
                    $" | [{(DateTime.UtcNow - args.Member.JoinedAt.UtcDateTime).Days} days ago]";
            }
            else
            {
                message += " - Joined: **Unknown**";
            }

            await sbgAug.SendMessageAsync($"{message}").ConfigureAwait(false);
        }
    }
}
