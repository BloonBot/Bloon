#pragma warning disable CA1822 // Mark members as static
namespace Bloon.Core.Commands
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Threading.Tasks;
    using Bloon.Core.Commands.Attributes;
    using Bloon.Features.IntruderBackend.Servers;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    public class OwnerCommands : BaseCommandModule
    {
        [Command("build")]
        [Description("Provides current build bloon is on.")]
        [OwnersExclusive]
        [Hidden]
        public Task BuildAsync(CommandContext ctx)
        {
            return ctx.RespondAsync($"{Assembly.GetEntryAssembly().GetName().Version}");
        }

        [Command("info")]
        [Description("Displays basic info and statistics about Bloon and this discord server")]
        [Hidden]
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
        [Hidden]
        public async Task SayAsync(CommandContext ctx, ulong channelID, [RemainingText] string message)
        {
            DiscordChannel channel = await ctx.Client.GetChannelAsync(channelID);
            await channel.SendMessageAsync(message);
            await ctx.RespondAsync($"Sent {message} to channel: {channel.Name}");
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
