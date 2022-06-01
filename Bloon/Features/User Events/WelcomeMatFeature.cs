namespace Bloon.Features.Doorman
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

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
            this.dClient.GuildMemberAdded -= this.GiveAgentRoleAsync;

            return base.Disable();
        }

        public override Task Enable()
        {
            this.dClient.GuildMemberAdded += this.GeneralWelcomeEmbed;
            this.dClient.GuildMemberAdded += this.GiveAgentRoleAsync;

            return base.Enable();
        }

        private async Task GeneralWelcomeEmbed(DiscordClient dClient, GuildMemberAddEventArgs args)
        {
            DiscordChannel sbgChannel = await this.dClient.GetChannelAsync(Channels.SBG.General);

            // If guild isn't SBG, just ignore this user join event.
            if (args.Guild.Id != Guilds.SBG)
            {
                return;
            }

            DiscordColor colorDate = new DiscordColor(95, 95, 95);

            // If the user's account age is less than 24 hours, we may be dealing with a throwaway/spam/etc. account.
            // Change their welcome embed color to flag these users as they may be malicious
            if ((DateTime.UtcNow - args.Member.CreationTimestamp.UtcDateTime).TotalHours <= 24)
            {
                colorDate = new DiscordColor(249, 183, 255);
            }

            DiscordEmbed embed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    Text = $"Account Created: {args.Member.CreationTimestamp.UtcDateTime.ToString("D", CultureInfo.InvariantCulture)}",
                },
                Color = colorDate,
                Timestamp = DateTime.UtcNow,
                Title = $"**New User Joined** | {args.Member.DisplayName}",
                Description = $"**User**: <@{args.Member.Id}>\n" +
                    $"**ID**: {args.Member.Id}",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = args.Member.AvatarUrl,
                },
            };

            await sbgChannel.SendMessageAsync(embed);
        }

        private async Task GiveAgentRoleAsync(DiscordClient dClient, GuildMemberAddEventArgs args)
        {
            // If guild isn't SBG, just ignore this user join event.
            if (args.Guild.Id != Guilds.SBG)
            {
                return;
            }

            await args.Member.GrantRoleAsync(args.Guild.GetRole(Roles.SBG.Agent));
            await args.Guild.GetChannel(Channels.SBG.Bloonside)
                .SendMessageAsync($"Granted **Agent** to **{args.Member.Username}**.");
        }
    }
}
