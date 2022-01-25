namespace Bloon.Features.Workshop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Bloon.Core.Commands.Attributes;
    using Bloon.Features.Workshop.Models;
    using Bloon.Variables.Emojis;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    [Group("maps")]
    [Aliases("workshop", "wmaps", "wmap")]
    [Description("Retrieves workshop data.")]
    [LimitedChannels]
    public class WorkshopCommands : BaseCommandModule
    {
        private readonly WorkshopService workshopService;

        public WorkshopCommands(WorkshopService workshopService)
        {
            this.workshopService = workshopService;
        }

        [Command("-top")]
        [Description("Displays the top 5 maps on the workshop based off current subscriptions.")]
        public async Task TopWorkshopMaps(CommandContext ctx)
        {
            List<WorkshopMap> maps = await this.workshopService.GetMapsFromDBAsync();

            DiscordEmbedBuilder workshopMapEmbed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(ctx.Client, PlatformEmojis.Steam).Url,
                    Text = "Workshop",
                },
                Color = new DiscordColor(0, 173, 238),
                Title = "Top Workshop Maps",
                Timestamp = DateTime.Now,
                Url = $"https://steamcommunity.com/app/518150/workshop/",
            };
            string description = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                description = description +
                    $"{DiscordEmoji.FromGuildEmote(ctx.Client, ServerEmojis.Map)} __[{maps.ElementAt(i).Title}](https://steamcommunity.com/sharedfiles/filedetails/?id={maps.ElementAt(i).FileID})__ " +
                    $"• {DiscordEmoji.FromGuildEmote(ctx.Client, ServerEmojis.Players)} __[{await this.workshopService.GetDBWorkshopMapCreator(maps.ElementAt(i).CreatorSteamID)}](https://steamcommunity.com/profiles/{maps.ElementAt(i).CreatorSteamID}/myworkshopfiles/?appid=518150)__ \n" +
                    $"{DiscordEmoji.FromName(ctx.Client, ":pushpin:")} Subscriptions: **{maps.ElementAt(i).Subscriptions}**\n" +
                    $"{DiscordEmoji.FromName(ctx.Client, ":heart:")} Favorites: **{maps.ElementAt(i).Favorited}**\n" +
                    $"{DiscordEmoji.FromGuildEmote(ctx.Client, EventEmojis.Join)} Followers: **{maps.ElementAt(i).Followers}**\n" +
                    $"\n";
            }

            workshopMapEmbed.Description = description;
            await ctx.RespondAsync(embed: workshopMapEmbed);
        }

        [OwnersExclusive]
        [Command("-scrape")]
        [Description("Populates the workshop_maps table.")]
        public async Task PopulateWorkshopMapsTable(CommandContext ctx)
        {
            List<WorkshopMap> maps = await this.workshopService.GetAllMapsAsync();
            await this.workshopService.StoreAllMaps(maps);
            await ctx.RespondAsync($"Finished repopulating table. Found {maps.Count} workshop maps.");
        }
    }
}
