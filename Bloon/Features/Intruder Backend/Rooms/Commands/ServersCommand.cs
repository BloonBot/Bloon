namespace Bloon.Features.IntruderBackend.Servers
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Bloon.Core.Commands.Attributes;
    using Bloon.Features.IntruderBackend.Rooms;
    using Bloon.Variables;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    [Group("servers")]
    [Aliases("gsi")]
    [Description("Retrieves all active servers.")]
    [LimitedChannels]
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
        public async Task CurrentServerInfo(CommandContext ctx)
        {
            CurrentServerInfo csi = await this.roomService.GetCSIRooms(null, null, null, null, "false", "false", "false", "false", "false", null, 1, 100);
            DiscordEmbedBuilder serverEmbed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Server.Unofficial).Url,
                },
                Color = new DiscordColor(217, 187, 19),
                Timestamp = DateTime.UtcNow,
            };
            serverEmbed.Url = "https://intruderfps.com/rooms";
            int skipRoomCount = 0;
            if (csi.Rooms.Count > 0)
            {
                StringBuilder serverList = new StringBuilder();
                int roomCount = 0;
                foreach (Rooms room in csi.Rooms)
                {
                    roomCount++;
                    if (roomCount >= 15)
                    {
                        skipRoomCount++;
                    }
                    else
                    {
                        serverList.AppendLine($"{room.ServerIcon} | {room.RegionFlag} | {room.Name} - [{room.AgentCount}/{room.MaxAgents}]");
                    }
                }

                if (skipRoomCount >= 1)
                {
                    serverList.Append($"<:unofficial:{Emojis.Server.Unofficial}> <:os:{Emojis.Server.Official}> <:passworded:{Emojis.Server.Password}> and **{skipRoomCount}** more servers.");
                }

                serverEmbed.Title = "Current Server Information";
                serverEmbed.Url = "https://intruderfps.com/rooms";
                serverEmbed.AddField($"Region | Name - [Agents]", serverList.ToString(), true);
            }
            else
            {
                serverEmbed.AddField($"Current Server Information", $"N/A", true);
            }

            string extensions = $"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Browser.Chrome)} [**Chrome**](https://chrome.google.com/webstore/detail/intruder-notifications/aoebpknpfcepopfgnbnikaipjeekalim) | "
                + $"[**Firefox**](https://addons.mozilla.org/en-US/firefox/addon/intruder-notifications/) {DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Browser.Firefox)}";
            serverEmbed.AddField("Browser Extensions", extensions);
            serverEmbed.Footer.Text = $"SuperbossGames | #current-server-info | Total Agents: {csi.PlayerCount}";

            await ctx.RespondAsync(embed: serverEmbed.Build());
        }

        [GroupCommand]
        public async Task CurrentServerInfo(CommandContext ctx, [RemainingText] string region)
        {
            region = ConvertRegion(region);

            CurrentServerInfo csi = await this.roomService.GetCSIRooms("true", "false", "false", "false", "false", "false", region);

            DiscordEmbedBuilder serverEmbed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Server.Unofficial).Url,
                },
                Color = new DiscordColor(217, 187, 19),
                Timestamp = DateTime.UtcNow,
            };
            int skipRoomCount = 0;
            if (csi.Rooms.Count > 0)
            {
                StringBuilder serverList = new StringBuilder();
                int roomCount = 0;
                foreach (Rooms room in csi.Rooms)
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
                    serverList.Append($"<:unofficial:{Emojis.Server.Unofficial}> <:os:{Emojis.Server.Official}> <:passworded:{Emojis.Server.Password}> and **{skipRoomCount}** more servers.");
                }

                serverEmbed.AddField($"Current Server Information", serverList.ToString(), true);
            }
            else
            {
                serverEmbed.AddField($"Current Server Information", $"N/A", true);
            }

            string extensions = $"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Browser.Chrome)} [**Chrome**](https://chrome.google.com/webstore/detail/intruder-notifications/aoebpknpfcepopfgnbnikaipjeekalim) | "
                + $"[**Firefox**](https://addons.mozilla.org/en-US/firefox/addon/intruder-notifications/) {DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Browser.Firefox)}";

            serverEmbed.AddField("Browser Extensions", extensions);
            serverEmbed.Footer.Text = $"SuperbossGames | #current-server-info | Total Agents: {csi.PlayerCount}";

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
