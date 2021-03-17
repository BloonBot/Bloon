namespace Bloon.Features.Workshop
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Services;
    using Bloon.Features.Workshop.Models;
    using Bloon.Utils;
    using Bloon.Variables.Channels;
    using Bloon.Variables.Emojis;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using Serilog;

    public class WorkshopJob : ITimedJob
    {
        private readonly DiscordClient dClient;
        private readonly WorkshopService workshopService;

        public WorkshopJob(DiscordClient dClient, WorkshopService workshopService)
        {
            this.dClient = dClient;
            this.workshopService = workshopService;
        }

        public ulong Emoji => PlatformEmojis.Steam;

        public int Interval => 5;

        public async Task Execute()
        {
            Log.Information("Checking Workshop for new maps..");

            // SocialItemWorkshopMap map = await this.workshopService.GetLatestAsync();
            List<WorkshopMap> maps = await this.workshopService.GetRecentlyUpdatedOrAddedAsync();

            if (maps.Count == 0)
            {
                // No new/updated maps.
                return;
            }

            DiscordChannel sbgGen = await this.dClient.GetChannelAsync(SBGChannels.General);
            DiscordChannel sbgMM = await this.dClient.GetChannelAsync(SBGChannels.MapMakerShowcase);

            DiscordEmbedBuilder workshopMapEmbed = new ()
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(this.dClient, PlatformEmojis.Steam).Url,
                    Text = "Workshop",
                },
                Color = new DiscordColor(0, 173, 238),
            };

            if (maps.Count == 1)
            {
                // Only 1 new/updated
                workshopMapEmbed.Timestamp = maps.ElementAt(0).TimeUpdated;
                workshopMapEmbed.Title = $"Workshop Update: {maps.ElementAt(0).Title}";
                workshopMapEmbed.Description = $"{DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Players)} __[{await this.workshopService.GetDBWorkshopMapCreator(maps.ElementAt(0).CreatorSteamID)}](https://steamcommunity.com/profiles/{maps.ElementAt(0).CreatorSteamID}/myworkshopfiles/?appid=518150)__ \n +" +
                    $"{maps.ElementAt(0).Description.Truncate(1000)}{(maps.ElementAt(0).Description.Length > 0 ? "..." : string.Empty)}";
                workshopMapEmbed.Url = $"https://steamcommunity.com/sharedfiles/filedetails/?id={maps.ElementAt(0).FileID}";
                workshopMapEmbed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = maps.ElementAt(0).PreviewURL.ToString(),
                };
            }

            if (maps.Count > 1)
            {
                // more than one map to post. Modify embed accordingly.
                workshopMapEmbed.Timestamp = maps.LastOrDefault().TimeUpdated;
                workshopMapEmbed.Title = $"Workshop - {maps.Count} Maps Updated or Added";
                string embedDescription = string.Empty;
                foreach (WorkshopMap workshopMap in maps)
                {
                    embedDescription = embedDescription + $"{DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Map)} __[{workshopMap.Title}](https://steamcommunity.com/sharedfiles/filedetails/?id={workshopMap.FileID})__ • {DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Players)} __[{await this.workshopService.GetDBWorkshopMapCreator(workshopMap.CreatorSteamID)}](https://steamcommunity.com/profiles/{workshopMap.CreatorSteamID}/myworkshopfiles/?appid=518150)__ \n" +
                        $"{workshopMap.Description.Truncate(256)}\n{(workshopMap.Description.Length > 0 ? "...\n" : string.Empty)}\n";
                }

                workshopMapEmbed.Description = embedDescription;
                workshopMapEmbed.Url = $"https://steamcommunity.com/app/518150/workshop/";
                workshopMapEmbed.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = "https://steamuserimages-a.akamaihd.net/ugc/783003963483229931/5BAD21E7A647DE0C275BB1E4A716BF3768E580B7/?imw=637&imh=358&ima=fit&impolicy=Letterbox&imcolor=%23000000",
                };
            }

            await sbgGen.SendMessageAsync(embed: workshopMapEmbed);
            await sbgMM.SendMessageAsync(embed: workshopMapEmbed);
            Log.Information("Finished Workshop Scraping");
        }
    }
}
