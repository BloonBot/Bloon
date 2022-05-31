namespace Bloon.Features.SteamNews
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using Serilog;

    public class SteamNewsJob : ITimedJob
    {
        private readonly DiscordClient dClient;
        private readonly BloonLog bloonLog;
        private readonly SteamNewsService newsService;

        public SteamNewsJob(DiscordClient dClient, BloonLog bloonLog, SteamNewsService newsService)
        {
            this.dClient = dClient;
            this.bloonLog = bloonLog;
            this.newsService = newsService;
        }

        public ulong Emoji => Emojis.Platform.Steam;

        public int Interval => 30;

        public async Task Execute()
        {
            Log.Information("Checking Steam for new announcements..");

            SteamNewsPost post = await this.newsService.GetLatestAsync();

            // Unable to fetch the latest announcement from Steam
            if (post == null)
            {
                this.bloonLog.Error($"Something went wrong fetching the latest Steam announcement! Check Log File");
                return;
            }
            else if (!await this.newsService.TryStoreNewAsync(post))
            {
                Log.Information("Finished Steam announcement checks early");
                return;
            }

            DiscordChannel sbgGen = await this.dClient.GetChannelAsync(Channels.SBG.General);

            DiscordEmbedBuilder mapEmbed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(this.dClient, Emojis.Platform.Steam).Url,
                    Text = "Announcement",
                },
                Color = new DiscordColor(0, 173, 238),
                Timestamp = post.Timestamp,
                Title = post.Title,
                Description = post.Description,
                Url = post.Url.ToString(),
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = "https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/apps/518150/d559137b4b294cf50053ecd5efb7a467754677d8.jpg",
                },
            };

            if (post.ImageUrl != null)
            {
                mapEmbed.ImageUrl = post.ImageUrl.ToString();
            }

            await sbgGen.SendMessageAsync(embed: mapEmbed);
            Log.Information("Finished Steam Announcement Scraping");
        }
    }
}
