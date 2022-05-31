namespace Bloon.Features.IntruderBackend.Servers
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Bloon.Core.Commands.Attributes;
    using Bloon.Features.IntruderBackend.Rooms;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.SlashCommands;

    [SlashModuleLifespan(SlashModuleLifespan.Scoped)]
    public class GSISlashCommand : ApplicationCommandModule
    {
        private readonly RoomService roomService;

        public GSISlashCommand(RoomService roomService)
        {
            this.roomService = roomService;
        }

        [SlashCommand("gsi", "Get server information.")]
        [SlashSBGExclusive]
        public async Task GSISlash(InteractionContext ctx)
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
                    if (roomCount >= 12)
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

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(serverEmbed.Build()).AsEphemeral(true));
        }
    }
}
