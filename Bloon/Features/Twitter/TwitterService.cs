namespace Bloon.Features.Twitter
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Core.Services;
    using LinqToTwitter;
    using LinqToTwitter.Common;
    using LinqToTwitter.OAuth;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    public class TwitterService : IDisposable, ISocialService<Status>
    {
        private readonly IServiceScopeFactory scopeFactory;
        private TwitterContext ctx;

        public TwitterService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        /// <summary>
        /// Logs into the Twitter service provider and authenticates bloon via the keys stored in the DB.
        /// </summary>
        public async void Authenticate()
        {
            // Keys and secrets could be moved to an external config file
            SingleUserAuthorizer auth = new SingleUserAuthorizer
            {
                CredentialStore = new InMemoryCredentialStore()
                {
                    ConsumerKey = Environment.GetEnvironmentVariable("TWITTER_CONSUMER_KEY"),
                    ConsumerSecret = Environment.GetEnvironmentVariable("TWITTER_CONSUMER_SECRET"),
                    OAuthToken = Environment.GetEnvironmentVariable("TWITTER_OAUTH_TOKEN"),
                    OAuthTokenSecret = Environment.GetEnvironmentVariable("TWITTER_OAUTH_TOKEN_SECRET"),
                },
            };

            await auth.AuthorizeAsync();

            this.ctx = new TwitterContext(auth);
        }

        /// <summary>
        /// Obtains the latest Twitter post from `Superbossgames` twibbler account.
        /// </summary>
        /// <param name="argument">Unused parameter.</param>
        /// <returns>Latest Tweet.</returns>
        public async Task<Status> GetLatestAsync(string argument = null)
        {
            Status tweet = await this.ctx.Status
                .Where(x => x.Type == StatusType.User && x.ScreenName == "Superbossgames" && x.TweetMode == TweetMode.Extended && x.ExcludeReplies == true && x.Count == 1)
                .FirstOrDefaultAsync();

            return tweet;
        }

        /// <summary>
        /// Try and store the Tweet in the database.
        /// </summary>
        /// <param name="tweet">Tweet.</param>
        /// <returns>Bool, if new or not.</returns>
        public async Task<bool> TryStoreNewAsync(Status tweet)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            if (db.Tweets.Any(x => x.UID == tweet.StatusID.ToString(CultureInfo.CurrentCulture)))
            {
                Log.Information("No new tweets");
                return false;
            }

            Log.Information("[TWITTER] New Tweet!");

            db.Tweets.Add(new Tweet()
            {
                UID = tweet.StatusID.ToString(CultureInfo.CurrentCulture),
                Author = tweet.User.Name,
                Title = tweet.FullText,
                Timestamp = tweet.CreatedAt.ToUniversalTime(),
            });
            await db.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Likes and favorites the tweet so that we can promote the user more.
        /// </summary>
        /// <param name="tweet">The tweet to like/favorite.</param>
        public async void LikeAndFavouriteThisShit(Status tweet)
        {
            try
            {
                await this.ctx.CreateFavoriteAsync(tweet.StatusID);
                await this.ctx.RetweetAsync(tweet.StatusID);
            }
            catch (TwitterQueryException e)
            {
                Log.Error(e, "Unable to like and favourite that shit (probably because Bloon already did so previously)");
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.ctx != null)
                {
                    this.ctx.Dispose();
                    this.ctx = null;
                }
            }
        }
    }
}
