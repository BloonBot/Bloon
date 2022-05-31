namespace Bloon.Features.Wiki
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;
    using DSharpPlus.SlashCommands;

    [SlashModuleLifespan(SlashModuleLifespan.Scoped)]
    public class WikiSlashCommands : ApplicationCommandModule
    {
        private readonly WikiService wikiService;

        public WikiSlashCommands(WikiService wikiService)
        {
            this.wikiService = wikiService;
        }

        [SlashCommand("wiki", "Retrieves wiki article.")]
        public async Task QueryWikiAsync(InteractionContext ctx, [Option("article", "The name or title of a particular wiki article.")] [RemainingText] string wikiArticle)
        {
            if (string.IsNullOrEmpty(wikiArticle))
            {
                await ctx.Channel.SendMessageAsync("Correct, we have a wiki.");
                return;
            }

            // Base embed
            DiscordEmbedBuilder wikiEmbed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Platform.Wiki).Url,
                    Text = "Superbossgames Wiki",
                },
                Color = new DiscordColor(217, 187, 19),
                Timestamp = DateTime.UtcNow,
            };

            WikiArticle article = await this.wikiService.GetArticleAsync(wikiArticle);

            // Found a matching article
            if (article != null)
            {
                wikiEmbed.Footer.Text += $" | Last Edited: {article.Timestamp}";
                wikiEmbed.Title = article.Title;
                wikiEmbed.Description = article.Body;
                wikiEmbed.Url = article.Url.ToString();
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(wikiEmbed.Build()));
                return;
            }

            // No exact match, find page titles containing the passed title
            List<string> suggestedPages = await this.wikiService.GetSuggestedPagesAsync(wikiArticle);

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

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(wikiEmbed.Build()).AsEphemeral(true));
        }
    }
}
