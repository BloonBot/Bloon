namespace Bloon.Features.IntruderBackend.Servers
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Bloon.Features.IntruderBackend.Rooms;
    using Bloon.Variables.Emojis;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;
    using IntruderLib.Models.Rooms;

    [Group("servers")]
    [Aliases("gsi")]
    [Description("Retrieves all active servers.")]
    public class ServersCommand : BaseCommandModule
    {
        private readonly RoomService roomService;

        public ServersCommand(RoomService roomService)
        {
            this.roomService = roomService;
        }

        /// <summary>
        /// Base #current-server-info command.
        /// </summary>
        /// <param name="ctx">Discord Client Context.</param>
        /// <returns>Server Embed via Command.</returns>
        [GroupCommand]
        public Task CurrentServerInfo(CommandContext ctx) => this.CurrentServerInfo(ctx, "ALL");

        [GroupCommand]
        public async Task CurrentServerInfo(CommandContext ctx, [RemainingText]string regionText)
        {
            CurrentServerInfo csi = await this.roomService.GetCSIRooms(new RoomListFilter
            {
                HideEmpty = true,
                Region = regionText == "ALL" ? null : Enum.Parse<Region>(ConvertRegion(regionText)),
            });

            DiscordEmbedBuilder serverEmbed = new ()
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(ctx.Client, ServerEmojis.Unofficial).Url,
                    Text = $"SuperbossGames | #current-server-info | Total Agents: {csi.PlayerCount}",
                },
                Color = new DiscordColor(217, 187, 19),
                Timestamp = DateTime.UtcNow,
            };
            int skipRoomCount = 0;
            if (csi.Rooms.Count > 0)
            {
                StringBuilder serverList = new ();
                int roomCount = 0;
                foreach (RoomDB room in csi.Rooms)
                {
                    if (roomCount >= 12)
                    {
                        skipRoomCount++;
                    }
                    else
                    {
                        serverList.AppendLine($"{room.ServerIcon} | {room.RegionFlag} | {room.Name} - [{room.AgentCount}/{room.MaxAgents}]");
                    }

                    roomCount++;
                }

                if (skipRoomCount >= 1)
                {
                    serverList.Append($"<:unofficial:{ServerEmojis.Unofficial}> <:os:{ServerEmojis.Official}> <:passworded:{ServerEmojis.Password}> and **{skipRoomCount}** more servers.");
                }

                serverEmbed.AddField($"Current Server Information", serverList.ToString(), true);
            }
            else
            {
                serverEmbed.AddField($"Current Server Information", $"N/A", true);
            }

            string extensions = $"{DiscordEmoji.FromGuildEmote(ctx.Client, BrowserEmojis.Chrome)} [**Chrome**](https://chrome.google.com/webstore/detail/intruder-notifications/aoebpknpfcepopfgnbnikaipjeekalim) | "
                + $"[**Firefox**](https://addons.mozilla.org/en-US/firefox/addon/intruder-notifications/) {DiscordEmoji.FromGuildEmote(ctx.Client, BrowserEmojis.Firefox)}";

            serverEmbed.AddField("Browser Extensions", extensions);

            await ctx.RespondAsync(embed: serverEmbed.Build());
        }

        private static string ConvertRegion(string region)
        {
            switch (region.ToLowerInvariant())
            {
                case "europe":
                case "eu":
                    region = "EU";
                    break;
                case "na":
                case "use":
                case "usw":
                case "united states":
                case "us":
                case "north america":
                    region = "US";
                    break;
                case "as":
                case "asia":
                    region = "Asia";
                    break;
                case "japan":
                case "jp":
                    region = "JP";
                    break;
                case "australia":
                case "aussie":
                case "au":
                    region = "AU";
                    break;
                case "southamerica":
                case "sa":
                    region = "SA";
                    break;
                case "cae":
                    region = "CAE";
                    break;
                case "korea":
                case "north korea":
                case "south korea":
                case "kr":
                    region = "KR";
                    break;
                case "india":
                case "in":
                    region = "IN";
                    break;
                case "motherland":
                case "russia":
                case "ru":
                    region = "RU";
                    break;
            }

            return region;
        }
    }
}
