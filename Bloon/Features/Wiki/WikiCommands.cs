#pragma warning disable CA1822 // Mark members as static
namespace Bloon.Features.Wiki
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Features.Wiki.Models;
    using Bloon.Utils;
    using Bloon.Variables.Emojis;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    [Group("wiki")]
    [Description("Retrieves a small description of a specified wiki article along with the URL where the information is pulled from.")]
    public class WikiCommands : BaseCommandModule
    {
        private readonly WikiService wikiService;

        public WikiCommands(WikiService wikiService)
        {
            this.wikiService = wikiService;
        }

        [GroupCommand]
        public async Task QueryWikiAsync(CommandContext ctx, [RemainingText]string pageTitle)
        {
            if (string.IsNullOrEmpty(pageTitle))
            {
                await ctx.Channel.SendMessageAsync("Correct, we have a wiki.").ConfigureAwait(false);
                return;
            }
            //else if (pageTitle.Equals("-rc", StringComparison.Ordinal))
            //{
            //    await this.ShowRecentChangesAsync(ctx).ConfigureAwait(false);
            //    return;
            //}
            //else if (pageTitle.Equals("-au", StringComparison.Ordinal))
            //{
            //    await this.ReturnActiveUsers(ctx).ConfigureAwait(false);
            //    return;
            //}

            // Base embed
            DiscordEmbedBuilder wikiEmbed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(ctx.Client, PlatformEmojis.Wiki).Url,
                    Text = "Superbossgames Wiki",
                },
                Color = new DiscordColor(217, 187, 19),
                Timestamp = DateTime.UtcNow,
            };

            WikiArticle article = await this.wikiService.GetArticleAsync(pageTitle).ConfigureAwait(false);

            // Found a matching article
            if (article != null)
            {
                wikiEmbed.Footer.Text += $" | Last Edited: {article.Timestamp}";
                wikiEmbed.Title = article.Title;
                wikiEmbed.Description = article.Body;
                wikiEmbed.Url = article.Url.ToString();
                await ctx.Channel.SendMessageAsync(string.Empty, embed: wikiEmbed.Build()).ConfigureAwait(false);
                return;
            }

            // No exact match, find page titles containing the passed title
            List<string> suggestedPages = await this.wikiService.GetSuggestedPagesAsync(pageTitle).ConfigureAwait(false);

            // Show what we found
            if (suggestedPages.Count > 0)
            {
                wikiEmbed.Title = "No exact match";
                wikiEmbed.Description = $"Are you looking for any of the following pages:\n"
                    + $"- {string.Join("\n- ", suggestedPages.Select(x => $"[{x}]({WikiUtils.GetUrlFromTitle(x)})"))}";
            }

            // No match, no suggestions, nothing
            else
            {
                wikiEmbed.Title = "Nothing found";
                wikiEmbed.Description = @"¯\_(ツ)_/¯";
            }

            await ctx.Channel.SendMessageAsync(embed: wikiEmbed.Build()).ConfigureAwait(false);
        }

        [Command("-recentchanges")]
        [Aliases("-rc")]
        [Description("Shows recent changes from the wiki. Default amount returned is 10 entries")]
        public async Task ShowRecentChangesAsync(CommandContext ctx)
        {
            List<RecentChanges> wikiArticles = await this.wikiService.GetRecentChanges().ConfigureAwait(false);

            if (wikiArticles.Count == 0)
            {
                await ctx.RespondAsync("No recent changes found!").ConfigureAwait(false);
                return;
            }

            string table = wikiArticles.Select(a => new
            {
                a.pageid,
                Type = a.type.Capitalize(),
                Title = a.title.Truncate(10),
                Author = a.user.Truncate(10),
                Timestamp = a.timestamp.ToString("g", CultureInfo.InvariantCulture),
            }).ToMarkdownTable();

            // Base embed
            DiscordEmbed wikiEmbed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(ctx.Client, PlatformEmojis.Wiki).Url,
                    Text = "Superbossgames Wiki",
                },
                Color = new DiscordColor(217, 187, 19),
                Timestamp = DateTime.UtcNow,
                Title = "Recent Wiki Changes",
                Description = $"```md\n{table}\n```",
            };

            await ctx.Channel.SendMessageAsync(embed: wikiEmbed).ConfigureAwait(false);
        }

        [Command("-activeusers")]
        [Aliases("-au")]
        [Description("Returns a list of active wiki editors within the past 30 days.")]
        public async Task ReturnActiveUsers(CommandContext ctx)
        {
            List<WikiUser> activeUsers = await this.wikiService.GetActiveUsers().ConfigureAwait(false);

            DiscordEmbedBuilder wikiEmbed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(ctx.Client, PlatformEmojis.Wiki).Url,
                    Text = "Superbossgames Wiki | Recent Users Last 30 Days",
                },
                Color = new DiscordColor(217, 187, 19),
                Timestamp = DateTime.UtcNow,
                Title = "Active Wiki Users",
            };

            foreach (WikiUser user in activeUsers)
            {
                wikiEmbed.AddField($"**__{user.Name}__**", $"**{user.CountRecentPosts}** *edits*", true);
            }

            await ctx.Channel.SendMessageAsync(embed: wikiEmbed.Build()).ConfigureAwait(false);
        }
    }
}
#pragma warning restore CA1822 // Mark members as static
