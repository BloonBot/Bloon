namespace Bloon.Features.IntruderBackend.Servers
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
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
        private readonly ActivityManager activityManager;
        private readonly RoomService roomService;

        public ServerJob(DiscordClient dClient, ActivityManager activityManager, RoomService roomService)
        {
            this.dClient = dClient;
            this.activityManager = activityManager;
            this.roomService = roomService;
        }

        public ulong Emoji => SBGEmojis.Superboss;

        public int Interval => 5;

        public async Task Execute()
        {
            Log.Information("Looking for available Intruder servers..");
            DiscordChannel sbgCSI = await this.dClient.GetChannelAsync(SBGChannels.CurrentServerInfo);
            CurrentServerInfo csi = await this.roomService.GetCSIRooms(null, null, null, null, "false", "true", "false", "false", "false", null, 1, 100);

            DiscordEmbedBuilder serverEmbed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Unofficial).Url,
                },
                Color = new DiscordColor(217, 187, 19),
                Timestamp = DateTime.UtcNow,
                Title = "Current Server Information",
                Url = "https://intruderfps.com/rooms",
            };
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
                serverEmbed.AddField(
                    $"Current Server Information",
                    $"Publicly no current servers available.\nUse `.ltp` to join the **Looking to Play** role",
                    true);
            }
#pragma warning disable SA1118 // Parameter should not span multiple lines
            serverEmbed.AddField(
                "Statistics",
                $"{RegionFlagEmojis.US}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.USTOD)}| **{csi.USPlayerCount}** {DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Players)}| **{csi.USRoomCount}** {DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Unofficial)}\n" +
                $"{RegionFlagEmojis.SA}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.SATOD)}| **{csi.SAPlayerCount}** {DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Players)}| **{csi.SARoomCount}** {DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Unofficial)}\n" +
                $"{RegionFlagEmojis.EU}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.EUTOD)}| **{csi.EUPlayerCount}** {DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Players)}| **{csi.EURoomCount}** {DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Unofficial)}\n" +
                $"{RegionFlagEmojis.RU}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.RUTOD)}| **{csi.RUPlayerCount}** {DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Players)}| **{csi.RURoomCount}** {DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Unofficial)}\n" +
                $"{RegionFlagEmojis.JP}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.JPTOD)}| **{csi.JPPlayerCount}** {DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Players)}| **{csi.JPRoomCount}** {DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Unofficial)}\n" +
                $"{RegionFlagEmojis.Asia}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.ASTOD)}| **{csi.AsiaPlayerCount}** {DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Players)}| **{csi.AsiaRoomCount}** {DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Unofficial)}\n" +
                $"{RegionFlagEmojis.AU}|{DiscordEmoji.FromGuildEmote(this.dClient, csi.AUTOD)}| **{csi.AUPlayerCount}** {DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Players)}| **{csi.AURoomCount}** {DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Unofficial)}\n" +
                $"Agents: **{csi.PlayerCount}** | Rooms: **{csi.Rooms.Count}**\n", true);
#pragma warning restore SA1118 // Parameter should not span multiple lines

            string extensions = $"{DiscordEmoji.FromGuildEmote(this.dClient, BrowserEmojis.Chrome)} [**Chrome**](https://chrome.google.com/webstore/detail/intruder-notifications/aoebpknpfcepopfgnbnikaipjeekalim) | "
                + $"[**Firefox**](https://addons.mozilla.org/en-US/firefox/addon/intruder-notifications/) {DiscordEmoji.FromGuildEmote(this.dClient, BrowserEmojis.Firefox)}";

            serverEmbed.AddField("Browser Extensions", extensions);

            serverEmbed.Footer.Text = $"SuperbossGames | #current-server-info";
            serverEmbed.Footer.IconUrl = DiscordEmoji.FromGuildEmote(this.dClient, ServerEmojis.Unofficial).Url;
            serverEmbed.Color = new DiscordColor(217, 187, 19);
            serverEmbed.Timestamp = DateTime.UtcNow;

            await this.activityManager.TrySetActivityAsync($"{csi.PlayerCount} agents ", ActivityType.Watching);

            try
            {
                foreach (DiscordMessage msg in await sbgCSI.GetMessagesAsync())
                {
                    if (msg.Author.Id == this.dClient.CurrentUser.Id)
                    {
                        await msg.ModifyAsync(embed: serverEmbed.Build());
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
