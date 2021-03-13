namespace Bloon.Features.Wiki
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables.Channels;
    using Bloon.Variables.Emojis;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using Serilog;

    public class WikiJob : ITimedJob
    {
        private readonly DiscordClient dClient;
        private readonly BloonLog bloonLog;
        private readonly WikiService wikiService;

        public WikiJob(DiscordClient dClient, BloonLog bloonLog, WikiService wikiService)
        {
            this.dClient = dClient;
            this.bloonLog = bloonLog;
            this.wikiService = wikiService;
        }

        public ulong Emoji => PlatformEmojis.Wiki;

        public int Interval => 5;

        public async Task Execute()
        {
            Log.Information("Checking Wiki for new posts..");

            WikiArticle article = await this.wikiService.GetLatestAsync().ConfigureAwait(false);

            // Unable to fetch the latest post from the wiki
            if (article == null)
            {
                this.bloonLog.Error($"Something went wrong fetching the latest wiki article! Check Log File");
                return;
            }
            else if (!await this.wikiService.TryStoreNewAsync(article).ConfigureAwait(false))
            {
                Log.Information("Finished Wiki checks early");
                return;
            }

            DiscordChannel sbgGen = await this.dClient.GetChannelAsync(SBGChannels.General).ConfigureAwait(false);
            DiscordChannel sbgWiki = await this.dClient.GetChannelAsync(SBGChannels.Wiki).ConfigureAwait(false);

            DiscordEmbed wikiEmbed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(this.dClient, PlatformEmojis.Wiki).Url,
                    Text = $"Superbossgames Wiki | Bytes Changed: {(article.ByteDifference > 0 ? "+" : string.Empty)}{article.ByteDifference}",
                },
                Color = new DiscordColor(217, 187, 19),
                Timestamp = article.Timestamp,
                Description = $"Recent Wiki Change by {article.Author} to [{article.Title}]({WikiUtils.GetUrlFromTitle(article.Title)})",
            };

            await sbgWiki.SendMessageAsync(embed: wikiEmbed).ConfigureAwait(false);
            await sbgGen.SendMessageAsync(embed: wikiEmbed).ConfigureAwait(false);

            Log.Information("Finished Wiki Scraping");
        }
    }
}
