namespace Bloon.Features.Youtube
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables.Channels;
    using Bloon.Variables.Emojis;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using Serilog;

    public class YouTubeJob : ITimedJob
    {
        private readonly DiscordClient dClient;
        private readonly BloonLog bloonLog;
        private readonly YouTubeService youTubeService;

        public YouTubeJob(DiscordClient dClient, BloonLog bloonLog, YouTubeService youTubeService)
        {
            this.dClient = dClient;
            this.bloonLog = bloonLog;
            this.youTubeService = youTubeService;
        }

        public ulong Emoji => PlatformEmojis.YouTube;

        public int Interval => 30;

        public async Task Execute()
        {
            Log.Information("Checking YouTube for new videos..");

            YouTubeVideo video = await this.youTubeService.GetLatestAsync();

            // Unable to fetch the latest post from youtube
            if (video == null)
            {
                this.bloonLog.Error($"Something went wrong fetching the latest youtube video! Check Log File");
                return;
            }
            else if (!await this.youTubeService.TryStoreNewAsync(video))
            {
                Log.Information("Finished Youtube checks early");
                return;
            }

            DiscordChannel sbgGen = await this.dClient.GetChannelAsync(SBGChannels.General);

            DiscordEmbed ytEmbed = new DiscordEmbedBuilder
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(this.dClient, SBGEmojis.Superboss).Url,
                    Name = "Superboss Games",
                    Url = "https://www.youtube.com/user/SuperbossGames",
                },
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(this.dClient, PlatformEmojis.YouTube).Url,
                    Text = "YouTube",
                },
                Color = new DiscordColor(255, 0, 0),
                Timestamp = video.Timestamp,
                Title = video.Title,
                Description = video.Description,
                Url = $"https://www.youtube.com/watch?v={video.UID}",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = video.ThumbnailUrl,
                },
            };

            await sbgGen.SendMessageAsync(embed: ytEmbed);
            Log.Information("Finished YouTube Scraping");
        }
    }
}
