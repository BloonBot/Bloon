namespace Bloon.Features.Twitter
{
    using System;
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Utils;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using LinqToTwitter;
    using Microsoft.Extensions.DependencyInjection;
    using Reddit;
    using Reddit.Controllers;
    using Serilog;

    public class TwitterJob : IDisposable, ITimedJob
    {
        private readonly DiscordClient dClient;
        private readonly BloonLog bloonLog;
        private readonly TwitterService twitterService;
        private readonly RedditClient redditAPI;

        public TwitterJob(IServiceScopeFactory scopeFactory, DiscordClient dClient, RedditClient redditAPI, BloonLog bloonLog)
        {
            this.dClient = dClient;
            this.bloonLog = bloonLog;
            this.twitterService = new TwitterService(scopeFactory);
            this.twitterService.Authenticate();
            this.redditAPI = redditAPI;
        }

        public ulong Emoji => Emojis.Platform.Twitter;

        public int Interval => 5;

        public async Task Execute()
        {
            Log.Information("Checking Twitter for new tweets..");

            Status tweet = await this.twitterService.GetLatestAsync();

            // Unable to fetch the latest tweet from twitter
            if (tweet == null)
            {
                this.bloonLog.Error($"Something went wrong fetching the latest tweet! Check Log File");
                return;
            }
            else if (!await this.twitterService.TryStoreNewAsync(tweet))
            {
                Log.Information("Finished Twitter checks early");
                return;
            }

            DiscordChannel sbgGen = await this.dClient.GetChannelAsync(Channels.SBG.General);

            DiscordEmbed tweetEmbed = new DiscordEmbedBuilder
            {
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    Name = $"{tweet.User.Name} (@{tweet.User.ScreenNameResponse})",
                    Url = $"https://twitter.com/{tweet.User.ScreenNameResponse}",
                },
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(this.dClient, Emojis.Platform.Twitter).Url,
                    Text = "Twitter",
                },
                Color = new DiscordColor(29, 161, 242),
                Timestamp = tweet.CreatedAt.ToUniversalTime(),
                Description = tweet.FullText,
            };

            await sbgGen.SendMessageAsync(embed: tweetEmbed);
            this.twitterService.LikeAndFavouriteThisShit(tweet);
            this.SendToReddit(tweet);
            Log.Information("Finished Twitter Scraping");
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SendToReddit(Status tweet)
        {
            Subreddit subreddit = this.redditAPI.Subreddit("Intruder").About();
            subreddit.LinkPost(tweet.FullText.Truncate(120), $"https://twitter.com/{tweet.User.ScreenNameResponse}/status/{tweet.StatusID}").Submit();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.twitterService != null)
                {
                    this.twitterService.Dispose();
                }
            }
        }
    }
}
