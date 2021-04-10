namespace Bloon.Features.Doorman
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Bloon.Core.Services;
    using Bloon.Variables.Channels;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;

    public class WelcomeMatFeature : Feature
    {
        private readonly DiscordClient dClient;

        public WelcomeMatFeature(DiscordClient dClient)
        {
            this.dClient = dClient;
        }

        public override string Name => "Welcome Mat";

        public override string Description => "Welcomes a user with a random phrase.";

        public override Task Disable()
        {
            this.dClient.GuildMemberAdded -= this.GeneralWelcomeEmbed;
            this.dClient.GuildMemberAdded -= this.BloonsideEmbed;

            return base.Disable();
        }

        public override Task Enable()
        {
            this.dClient.GuildMemberAdded += this.GeneralWelcomeEmbed;
            this.dClient.GuildMemberAdded += this.BloonsideEmbed;

            return base.Enable();
        }

        private async Task GeneralWelcomeEmbed(DiscordClient dClient, GuildMemberAddEventArgs args)
        {
            DiscordChannel sbgChannel = await this.dClient.GetChannelAsync(SBGChannels.General);
            DiscordColor colorDate = new DiscordColor(95, 95, 95);

            // If guild isn't SBG, just ignore this user join event.
            if (args.Guild.Id != Variables.Guilds.SBG)
            {
                return;
            }

            // If the user's account age is less than 24 hours, we may be dealing with a throwaway/spam/etc. account.
            // Change their welcome embed color to flag these users as they may be malicious
            if ((DateTime.UtcNow - args.Member.CreationTimestamp.UtcDateTime).TotalHours <= 24)
            {
                colorDate = new DiscordColor(249, 183, 255);
            }

            DiscordEmbed userDetails = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    Text = $"Account Created: {args.Member.CreationTimestamp.UtcDateTime.ToString("D", CultureInfo.InvariantCulture)}",
                },
                Color = colorDate,
                Timestamp = DateTime.UtcNow,
                Title = $"**New User Joined** | {args.Member.DisplayName}",
                Description = $"**User**: {args.Member.Mention}\n" +
                    $"**ID**: {args.Member.Id}\n",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = args.Member.AvatarUrl,
                },
            };

            await sbgChannel.SendMessageAsync(string.Empty, embed: userDetails);
        }

        private async Task BloonsideEmbed(DiscordClient dClient, GuildMemberAddEventArgs args)
        {
            DiscordChannel sbgChannel = await this.dClient.GetChannelAsync(SBGChannels.Bloonside);
            DiscordColor colorDate = new DiscordColor(95, 95, 95);

            // If guild isn't SBG, just ignore this user join event.
            if (args.Guild.Id != Variables.Guilds.SBG)
            {
                return;
            }

            // If the user's account age is less than 24 hours, we may be dealing with a throwaway/spam/etc. account.
            // Change their welcome embed color to flag these users as they may be malicious
            if ((DateTime.UtcNow - args.Member.CreationTimestamp.UtcDateTime).TotalHours <= 24)
            {
                colorDate = new DiscordColor(249, 183, 255);
            }

            DiscordEmbed userDetails = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    Text = $"Account Created: {args.Member.CreationTimestamp.UtcDateTime.ToString("D", CultureInfo.InvariantCulture)}",
                },
                Color = colorDate,
                Timestamp = DateTime.UtcNow,
                Title = $"**New User Joined** | {args.Member.DisplayName}",
                Description = $"**User**: {args.Member.Mention}\n" +
                    $"**ID**: {args.Member.Id}\n" +
                    $"User Events: N/A",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = args.Member.AvatarUrl,
                },
            };

            // Post embed to #bloonside
            await sbgChannel.SendMessageAsync(string.Empty, embed: userDetails);
        }
    }
}
