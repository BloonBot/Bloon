#pragma warning disable CA1822 // Mark members as static
namespace Bloon.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Threading.Tasks;
    using Bloon.Core.Commands.Attributes;
    using Bloon.Features.IntruderBackend.Rooms;
    using Bloon.Features.IntruderBackend.Servers;
    using Bloon.Variables.Channels;
    using Bloon.Variables.Emojis;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    public class OwnerCommands : BaseCommandModule
    {
        private readonly RoomService roomService;

        public OwnerCommands(RoomService roomService)
        {
            this.roomService = roomService;
        }

        [Command("build")]
        [Description("Provides current build bloon is on.")]
        [OwnersExclusive]
        public Task BuildAsync(CommandContext ctx)
        {
            return ctx.RespondAsync($"{Assembly.GetEntryAssembly().GetName().Version}");
        }

        [Command("info")]
        [Description("Displays basic info and statistics about Bloon and this discord server")]
        public Task Info(CommandContext ctx)
        {
            return ctx.RespondAsync(
                $"{Formatter.Bold("Info")}\n" +
                $"- Heap Size: {GetHeapSize()} MB\n" +
                $"- Library: Discord.Net ({ctx.Client.VersionString})\n" +
                $"- Uptime: {GetUptime()}\n");
        }

        [Command("say")]
        [Description("Make Bloon send a message in any channel.")]
        [OwnersExclusive]
        public async Task SayAsync(CommandContext ctx, ulong channelID, [RemainingText] string message)
        {
            DiscordChannel channel = await ctx.Client.GetChannelAsync(channelID);
            await channel.SendMessageAsync(message);
            await ctx.RespondAsync($"Sent {message} to channel: {channel.Name}");
        }

        [Command("rooms")]
        public async Task GoGetRooms(CommandContext ctx)
        {
            List<Rooms> rooms = await this.roomService.GetRooms(null, null, null, null, null, null, null, null, null, null, 1, 100);
            await this.roomService.ArchiveRoomData(rooms);

            // await this.roomService.ArchiveRoomMapHistory(rooms);
            await ctx.RespondAsync($"{rooms.Count}");
        }

        [OwnersExclusive]
        [Command("welcomeagents")]
        public async Task UpdateWelcomeAgents(CommandContext ctx)
        {
            // DiscordChannel sbgBugs = await this.dClient.GetChannelAsync(SBGChannels.Bugs);
            DiscordEmbedBuilder hrEmbed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(23, 153, 177),
                Timestamp = DateTime.Now,
                Title = "Welcome new Agents!",
            };

#pragma warning disable SA1118 // Parameter should not span multiple lines
            hrEmbed.AddField(
                "Our Channels",
                $"<#{SBGChannels.Announcements}> - get the latest news for Intruder here\n" +
                $"<#{SBGChannels.CurrentServerInfo}> - see what servers are running on Intruder right now\n" +
                $"<#{SBGChannels.Help}> - come here to get technical help with your game or account\n" +
                $"<#{SBGChannels.General}> - come here for general chat with players and developers\n" +
                $"<#{SBGChannels.Mapmaker}> - come here for Intruder Map Maker help and design ideas\n" +
                $"<#{SBGChannels.PicsNVids}> - check out and add your own Intruder pics, videos, and gifs\n" +
                $"<#{SBGChannels.Wiki}> - come here to ask about the wiki or to learn to edit it yourself\n" +
                $"<#{SBGChannels.Bloonside}> - use my commands here so you don't clutter the other chats", false);

            hrEmbed.AddField(
                "Important Links",
                $"{SBGEmojis.Superboss}[**Homepage**](https://intruderfps.com/)\n" +
                $"{PlatformEmojis.YouTube}[**Youtube**](https://www.youtube.com/superbossgames)\n" +
                $"{PlatformEmojis.Reddit}[**Reddit Page**](https://www.reddit.com/r/Intruder)\n" +
                $"{PlatformEmojis.Twitter}[**Twitter**](https://twitter.com/SuperbossGames/)\n" +
                $"{PlatformEmojis.Steam}[**Steam Group**](https://steamcommunity.com/groups/SuperbossGames)\n", false);

            hrEmbed.AddField(
                "Other Links",
                $"{SBGEmojis.Superboss}[**Homepage**](https://intruderfps.com/)\n" +
                $"{PlatformEmojis.YouTube}[**Youtube**](https://www.youtube.com/superbossgames)\n" +
                $"{PlatformEmojis.Reddit}[**Reddit Page**](https://www.reddit.com/r/Intruder)\n" +
                $"{PlatformEmojis.Twitter}[**Twitter**](https://twitter.com/SuperbossGames/)\n" +
                $"{PlatformEmojis.Steam}[**Steam Group**](https://steamcommunity.com/groups/SuperbossGames)\n", false);
#pragma warning restore SA1118 // Parameter should not span multiple lines

            string extensions = $"{DiscordEmoji.FromGuildEmote(ctx.Client, BrowserEmojis.Chrome)} [**Chrome**](https://chrome.google.com/webstore/detail/intruder-notifications/aoebpknpfcepopfgnbnikaipjeekalim) | "
                + $"[**Firefox**](https://addons.mozilla.org/en-US/firefox/addon/intruder-notifications/) {DiscordEmoji.FromGuildEmote(ctx.Client, BrowserEmojis.Firefox)}";

            hrEmbed.AddField("Browser Extensions", extensions);

            await ctx.RespondAsync(embed: hrEmbed.Build());

            // DiscordMessage message = await sbgBugs.SendMessageAsync(embed: hrEmbed);
        }

        private static string GetUptime()
        {
            return (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss", CultureInfo.InvariantCulture);
        }

        private static string GetHeapSize()
        {
            return Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString(CultureInfo.InvariantCulture);
        }
    }
}
#pragma warning restore CA1822 // Mark members as static
