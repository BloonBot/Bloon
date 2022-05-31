namespace Bloon.Features.Helprace
{
    using System;
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Utils;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using Serilog;

    public class HelpraceJob : ITimedJob
    {
        private readonly DiscordClient dClient;
        private readonly BloonLog bloonLog;
        private readonly HelpraceService helpraceService;

        public HelpraceJob(DiscordClient dClient, BloonLog bloonLog, HelpraceService helpraceService)
        {
            this.dClient = dClient;
            this.bloonLog = bloonLog;
            this.helpraceService = helpraceService;
        }

        public ulong Emoji => Emojis.Platform.Helprace;

        public int Interval => 5;

        public async Task Execute()
        {
            Log.Information("Checking Helprace for new posts..");

            HelpracePost post = await this.helpraceService.GetLatestAsync("all_topics");

            // Unable to fetch the latest post from helprace
            if (post == null)
            {
                this.bloonLog.Error($"Something went wrong fetching the latest helprace post! Check Log File");
                return;
            }
            else if (!await this.helpraceService.TryStoreNewAsync(post))
            {
                Log.Information("Finished Helprace checks early");
                return;
            }

            DiscordChannel sbgBugs = await this.dClient.GetChannelAsync(Channels.SBG.Bugs);

            if (post.Title.Length > 128)
            {
                post.Title = post.Title.Truncate(128) + "...";
            }

            if (post.Body.Length > 256)
            {
                post.Body = post.Body.Truncate(256) + $"... [Read more](https://superbossgames.helprace.com/i{post.UID})";
            }

            DiscordEmbed hrEmbed = new DiscordEmbedBuilder
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    Name = post.Author,
                    Url = $"https://superbossgames.helprace.com/i{post.UID}",
                },
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(this.dClient, Emojis.Platform.Helprace).Url,
                    Text = $"Helprace | Channel: {post.Channel.Replace("ideas", "Suggestions", StringComparison.Ordinal).Replace("problems", "Bugs", StringComparison.Ordinal)}",
                },
                Color = new DiscordColor(23, 153, 177),
                Timestamp = post.Timestamp,
                Title = post.Title,
                Description = post.Body,
                Url = $"https://superbossgames.helprace.com/i{post.UID}",
            };

            DiscordMessage message = await sbgBugs.SendMessageAsync(embed: hrEmbed);
            await message.CreateReactionAsync(DiscordEmoji.FromGuildEmote(this.dClient, Emojis.Reputation.Up));
            await message.CreateReactionAsync(DiscordEmoji.FromGuildEmote(this.dClient, Emojis.Reputation.Down));
            Log.Information("Finished Helprace Scraping");
        }
    }
}
