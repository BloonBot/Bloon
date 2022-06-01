namespace Bloon.Commands
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Bloon.Core.Commands.Attributes;
    using Bloon.Core.Discord;
    using Bloon.Utils;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;
    using Serilog;
    using TwitchLib.Api;
    using TwitchLib.Api.V5.Models.Streams;
    using TwitchLib.Api.V5.Models.Users;

    [Group("stream")]
    [Description("Manage bloon's streaming status. This currently only supports Twitch streams.")]
    [LimitedChannels]
    public class StreamCommands : BaseCommandModule
    {
        private readonly TwitchAPI twitchAPI;
        private readonly ActivityManager activityManager;

        public StreamCommands(ActivityManager activityManager, TwitchAPI twitchAPI)
        {
            this.activityManager = activityManager;
            this.twitchAPI = twitchAPI;
        }

        [GroupCommand]
        [Description("Sets the Stream")]
#pragma warning disable CA1054 // Uri parameters should not be strings
        public async Task SetStream(CommandContext ctx, [Description("Stream Url")] string url)
#pragma warning restore CA1054 // Uri parameters should not be strings
        {
            // Dumb Discord.NET
            if (url.StartsWith('-'))
            {
                await ctx.RespondAsync("Invalid stream url");
                return;
            }
            else if (ctx.Client.CurrentUser.Presence.Activity.ActivityType == ActivityType.Streaming)
            {
                await ctx.RespondAsync("Sorry, I'm already watching somebody else's stream");
                return;
            }

            string name = url;

            // Probably a url
            if (url.Contains('.', StringComparison.Ordinal))
            {
                Match match = Regex.Match(url, @"\.tv\/([^\/]*)");

                if (!match.Success)
                {
                    await ctx.RespondAsync("Unable to process stream url");
                    return;
                }

                name = match.Groups[1].Value;
            }

            Users users;

            try
            {
                users = await this.twitchAPI.V5.Users.GetUserByNameAsync(name);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Log.Error(e, "Unable to grab Twitch users");
                await ctx.RespondAsync("Invalid stream url");
                return;
            }

            if (users.Matches.Length == 0)
            {
                await ctx.RespondAsync($"Streamer not found");
                return;
            }

            StreamByUser userStream = await this.twitchAPI.V5.Streams.GetStreamByUserAsync(users.Matches[0].Id);

            if (userStream.Stream == null)
            {
                await ctx.RespondAsync($"Stream seems to be offline");
                return;
            }
            else if (userStream.Stream.Game != "Intruder")
            {
                await ctx.RespondAsync($"I only promote **Intruder**{(userStream.Stream.Game.Length > 0 ? $", not **{userStream.Stream.Game}**," : string.Empty)} streams");
                return;
            }

            await this.activityManager.SetStreamAsync(ctx.User.Id, name.Capitalize(), $"https://twitch.tv/{name}");
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":heavy_check_mark:"));
        }

        [Command("-clear")]
        [Aliases("-c")]
        [Priority(1)] // Increase priority, so the command isn't passed as a parameter to the above one
        [Description("Clears the Stream")]
        public async Task ClearStream(CommandContext ctx)
        {
            await this.activityManager.ClearStreamAsync();
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":heavy_check_mark:"));
        }
    }
}
