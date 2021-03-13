namespace Bloon.Features.IntruderBackend.Servers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Bloon.Core.Services;
    using Bloon.Features.IntruderBackend.Rooms;
    using Bloon.Variables.Channels;
    using Bloon.Variables.Emojis;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using Serilog;

    public class ServerJob : ITimedJob
    {
        private readonly DiscordClient dClient;
        private readonly RoomService roomService;

        public ServerJob(DiscordClient dClient, RoomService roomService)
        {
            this.dClient = dClient;
            this.roomService = roomService;
        }

        public ulong Emoji => SBGEmojis.Superboss;

        public int Interval => 5;

        public async Task Execute()
        {
            Log.Information("Looking for available Intruder servers..");
            DiscordChannel sbgCSI = await this.dClient.GetChannelAsync(SBGChannels.CurrentServerInfo).ConfigureAwait(false);
            CurrentServerInfo csi = await this.roomService.GetCSIRooms(null, null, null, null, "false", "true", "false", "false", "false", null, 1, 100).ConfigureAwait(false);

            DiscordEmbedBuilder serverEmbed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Unofficial).Url,
                },
                Color = new DiscordColor(217, 187, 19),
                Timestamp = DateTime.UtcNow,
            };
            serverEmbed.Title = "Current Server Information";
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
                    serverList.Append($"<:unofficial:{ServerEmojis.Unofficial}> <:os:{ServerEmojis.Official}> <:passworded:{ServerEmojis.Password}> and **{skipRoomCount}** more.");
                }

                serverEmbed.AddField($"Server | Region | Name - [Agents]", serverList.ToString(), true);
            }
            else
            {
                serverEmbed.AddField($"Current Server Information",
                    $"Publicly no current servers available.\n" +
                    $"Use `.ltp` to join the **Looking to Play** role", true);
            }
#pragma warning disable SA1118 // Parameter should not span multiple lines
            serverEmbed.AddField("Statistics",
               // $"{RegionFlagEmojis.CAE}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.CAETOD)}|**{csi.CAEPlayerCount}** agents | **{csi.CAERoomCount}** Rooms\n" +
                $"{RegionFlagEmojis.US}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.USTOD)}| **{csi.USPlayerCount}** agents | **{csi.USRoomCount}** Rooms\n" +
                $"{RegionFlagEmojis.SA}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.SATOD)}| **{csi.SAPlayerCount}** agents | **{csi.SARoomCount}** Rooms\n" +
                $"{RegionFlagEmojis.EU}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.EUTOD)}| **{csi.EUPlayerCount}** agents | **{csi.EURoomCount}** Rooms\n" +
                $"{RegionFlagEmojis.RU}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.RUTOD)}| **{csi.RUPlayerCount}** agents | **{csi.RURoomCount}** Rooms\n" +
                //$"{RegionFlagEmojis.IN}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.INTOD)}|**{csi.INPlayerCount}** agents | **{csi.INRoomCount}** Rooms\n" +
                //$"{RegionFlagEmojis.Asia}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.ASTOD)}|**{csi.AsiaPlayerCount}** agents | **{csi.AsiaRoomCount}** Rooms\n" +
                $"{RegionFlagEmojis.JP}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.JPTOD)}| **{csi.JPPlayerCount}** agents | **{csi.JPRoomCount}** Rooms\n" +
                //$"{RegionFlagEmojis.KR}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.KRTOD)}| **{csi.KRPlayerCount}** agents | **{csi.KRRoomCount}** Rooms\n" +
                $"{RegionFlagEmojis.AU}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.AUTOD)}| **{csi.AUPlayerCount}** agents | **{csi.AURoomCount}** Rooms\n" +
                $"Players: **{csi.PlayerCount}** | Servers: **{csi.Rooms.Count}**\n", true);
#pragma warning restore SA1118 // Parameter should not span multiple lines


            string extensions = $"{DiscordEmoji.FromGuildEmote(this.dClient, BrowserEmojis.Chrome)} [**Chrome**](https://chrome.google.com/webstore/detail/intruder-notifications/aoebpknpfcepopfgnbnikaipjeekalim) | "
                + $"[**Firefox**](https://addons.mozilla.org/en-US/firefox/addon/intruder-notifications/) {DiscordEmoji.FromGuildEmote(this.dClient, BrowserEmojis.Firefox)}";

            serverEmbed.AddField("Browser Extensions", extensions);

            serverEmbed.Footer.Text = $"SuperbossGames | #current-server-info";
            serverEmbed.Footer.IconUrl = DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Unofficial).Url;
            serverEmbed.Color = new DiscordColor(217, 187, 19);
            serverEmbed.Timestamp = DateTime.UtcNow;

            try
            {
                foreach (DiscordMessage msg in await sbgCSI.GetMessagesAsync().ConfigureAwait(false))
                {
                    if (msg.Author.Id == this.dClient.CurrentUser.Id)
                    {
                        await msg.ModifyAsync(embed: serverEmbed.Build()).ConfigureAwait(false);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e.InnerException, "Failed to edit or update the Current Server Info.");
            }

            Log.Information("Finished looking for Intruder Servers");
        }
    }
}
