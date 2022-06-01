namespace Bloon.Features.RedditGuard
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using Microsoft.Extensions.DependencyInjection;
    using Reddit;
    using Reddit.Controllers;
    using Reddit.Controllers.EventArgs;
    using Serilog;

    public class RedditMonitor
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly DiscordClient dClient;
        private readonly Subreddit subreddit;

        public RedditMonitor(IServiceScopeFactory scopeFactory, DiscordClient dClient, RedditClient rClient)
        {
            this.scopeFactory = scopeFactory;
            this.dClient = dClient;
            this.subreddit = rClient.Subreddit("Intruder").About();
        }

        public void Register()
        {
            // If we don't fetch the latest posts first, an event is fired with ALL new posts
            this.subreddit.Posts.GetNew();
            this.subreddit.Posts.NewUpdated += this.NewUpdatedAsync;
            bool monitoring = this.subreddit.Posts.MonitorNew();

            if (!monitoring)
            {
                this.subreddit.Posts.MonitorNew();
            }

            Log.Information("[Reddit] Monitoring new posts");
        }

        public void Unregister()
        {
            bool monitoring = this.subreddit.Posts.MonitorNew();

            if (monitoring)
            {
                this.subreddit.Posts.MonitorNew();
            }

            this.subreddit.Posts.NewUpdated -= this.NewUpdatedAsync;
            Log.Information("[Reddit] Stopped monitoring new posts");
        }

        private async void NewUpdatedAsync(object sender, PostsUpdateEventArgs posts)
        {
            await this.NewUpdatedAsync(posts.Added);
        }

        /// <summary>
        /// Finds the latest, new updated RedditPost. Should trigger whenever Guardbot can see the post.
        /// </summary>
        /// <param name="posts">Reddit Posts.</param>
        /// <returns>Reddit Embed containing Newly Discovered Post.</returns>
        private async Task NewUpdatedAsync(List<Post> posts)
        {
            if (posts.Count == 0)
            {
                Log.Information($"[REDDIT] No news posts!");
                return;
            }

            Log.Information("[REDDIT] Received {0} new post(s)!", posts.Count);

            DiscordChannel sbgGen = await this.dClient.GetChannelAsync(Channels.SBG.General);

            foreach (Post post in posts)
            {
                if (post.Author == "GuardBloon")
                {
                    continue;
                }

                string postUrl = "https://reddit.com" + post.Permalink;

                DiscordEmbed redditEmbed = new DiscordEmbedBuilder
                {
                    Author = new DiscordEmbedBuilder.EmbedAuthor()
                    {
                        Name = post.Author,
                        Url = postUrl,
                    },
                    Footer = new DiscordEmbedBuilder.EmbedFooter()
                    {
                        IconUrl = DiscordEmoji.FromGuildEmote(this.dClient, Emojis.Platform.Reddit).Url,
                        Text = "/r/Intruder",
                    },
                    Color = new DiscordColor(255, 69, 0),
                    Timestamp = post.Created.ToUniversalTime(),
                    Description = $"[{post.Title}]({postUrl})",
                };

                using IServiceScope scope = this.scopeFactory.CreateScope();
                using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
                db.RedditPosts.Add(new RedditPost()
                {
                    UID = post.Id,
                    Author = post.Author,
                    Timestamp = post.Created.ToUniversalTime(),
                    Title = post.Title,
                });

                await db.SaveChangesAsync();

                if (string.IsNullOrEmpty(post.Listing.Thumbnail)
                    || post.Listing.Thumbnail == "self"
                    || post.Listing.Thumbnail == "default")
                {
                    await sbgGen.SendMessageAsync(embed: redditEmbed);
                }
                else
                {
                    DiscordChannel sbgPNV = await this.dClient.GetChannelAsync(Channels.SBG.PicsNVids);

                    redditEmbed = new DiscordEmbedBuilder(redditEmbed)
                    {
                        Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                        {
                            Url = post.Listing.Thumbnail,
                        },
                    };

                    await sbgPNV.SendMessageAsync(embed: redditEmbed);
                }
            }

            Log.Information($"[REDDIT] Finished processing new posts!");
        }
    }
}
