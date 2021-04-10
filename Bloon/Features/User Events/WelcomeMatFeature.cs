namespace Bloon.Features.Doorman
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Analytics.Users;
    using Bloon.Core.Database;
    using Bloon.Core.Services;
    using Bloon.Variables.Channels;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public class WelcomeMatFeature : Feature
    {
        private readonly IServiceProvider provider;
        private readonly DiscordClient dClient;

        private readonly Dictionary<Event, ulong> eventEmotes = new Dictionary<Event, ulong> {
            { Event.Banned, Variables.Emojis.EventEmojis.Banned },
            { Event.Joined, Variables.Emojis.EventEmojis.Join },
            { Event.Left, Variables.Emojis.EventEmojis.Leave },
            { Event.Unbanned, Variables.Emojis.EventEmojis.Edited },
        };

        public WelcomeMatFeature(IServiceProvider provider, DiscordClient dClient)
        {
            this.provider = provider;
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

            // If guild isn't SBG, just ignore this user join event.
            if (args.Guild.Id != Variables.Guilds.SBG)
            {
                return;
            }

            await sbgChannel.SendMessageAsync(this.BuildEmbed(args.Member));
        }

        private async Task BloonsideEmbed(DiscordClient dClient, GuildMemberAddEventArgs args)
        {
            DiscordChannel sbgChannel = await this.dClient.GetChannelAsync(SBGChannels.Bloonside);

            // If guild isn't SBG, just ignore this user join event.
            if (args.Guild.Id != Variables.Guilds.SBG)
            {
                return;
            }

            DiscordEmbedBuilder embed = this.BuildEmbed(args.Member);

            using IServiceScope scope = this.provider.CreateScope();
            using AnalyticsContext db = scope.ServiceProvider.GetRequiredService<AnalyticsContext>();
            List<UserEvent> userEvents = await db.UserEvents
                .Where(u => u.UserId == args.Member.Id)
                .OrderByDescending(u => u.Timestamp)
                .Take(10)
                .ToListAsync();

            embed.AddField(
                "Events",
                string.Join(
                    "\n",
                    userEvents.Select(u => $"{DiscordEmoji.FromGuildEmote(this.dClient, this.eventEmotes[u.Event])} {u.Event.ToString().PadRight(8, '\u2000')} - {u.Timestamp.ToString("ddd, dd MMM yyyy, hh:mm:ss tt", CultureInfo.InvariantCulture)}")));

            // Post embed to #bloonside
            await sbgChannel.SendMessageAsync(embed);
        }

        private DiscordEmbedBuilder BuildEmbed(DiscordMember member)
        {
            DiscordColor colorDate = new DiscordColor(95, 95, 95);

            // If the user's account age is less than 24 hours, we may be dealing with a throwaway/spam/etc. account.
            // Change their welcome embed color to flag these users as they may be malicious
            if ((DateTime.UtcNow - member.CreationTimestamp.UtcDateTime).TotalHours <= 24)
            {
                colorDate = new DiscordColor(249, 183, 255);
            }

            return new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    Text = $"Account Created: {member.CreationTimestamp.UtcDateTime.ToString("D", CultureInfo.InvariantCulture)}",
                },
                Color = colorDate,
                Timestamp = DateTime.UtcNow,
                Title = $"**New User Joined** | {member.DisplayName}",
                Description = $"**User**: {member.Mention}\n" +
                    $"**ID**: {member.Id}",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = member.AvatarUrl,
                },
            };
        }
    }
}
