namespace Bloon.Features.TwitchMarley
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Commands;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Utils;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using Serilog;
    using TwitchLib.Api;
    using TwitchLib.Api.V5.Models.Search;
    using TwitchLib.Api.V5.Models.Streams;
    using TwitchLib.Client;
    using TwitchLib.Client.Events;
    using TwitchLib.Client.Models;

    public class TwitchMarleyFeature : Feature
    {
        private readonly ActivityManager activityManager;
        private readonly CommandsNextExtension cNext;
        private readonly DiscordClient dClient;
        private readonly StreamEvents streamEvents;
        private readonly TwitchAPI twitchAPI;
        private readonly TwitchClient tClient;

        public TwitchMarleyFeature(ActivityManager activityManager, DiscordClient dClient, TwitchAPI twitchAPI, TwitchClient twitchClient)
        {
            this.activityManager = activityManager;
            this.cNext = dClient.GetCommandsNext();
            this.dClient = dClient;
            this.streamEvents = new StreamEvents(activityManager, twitchAPI);
            this.twitchAPI = twitchAPI;
            this.tClient = twitchClient;
        }

        public override string Name => "Twitch";

        public override string Description => "Manages the now Streaming users if they are Streaming the game on Twitch.";

        public override Task Disable()
        {
            this.cNext.UnregisterCommands<StreamCommands>();
            this.dClient.PresenceUpdated -= this.ManageNowStreamingUsers;
            this.streamEvents.Unregister();
            this.tClient.Disconnect();

            return base.Disable();
        }

        public override Task Enable()
        {
            this.cNext.RegisterCommands<StreamCommands>();
            this.dClient.PresenceUpdated += this.ManageNowStreamingUsers;
            this.streamEvents.Register();
            this.tClient.Connect();

            return base.Enable();
        }

        public override Task Initialize()
        {
            this.tClient.Initialize(new ConnectionCredentials(Environment.GetEnvironmentVariable("TWITCH_USER"), Environment.GetEnvironmentVariable("TWITCH_OAUTH_TOKEN")), "Superbossgames");
            this.tClient.OnLog += this.ConvertToSerilog;

            AppDomain.CurrentDomain.ProcessExit += this.OnShutdown;

            return base.Initialize();
        }

        private void ConvertToSerilog(object sender, OnLogArgs args)
        {
            Log.Information("[Twitch] {BotUsername} : {Data}", args.BotUsername, args.Data);
        }

        private void OnShutdown(object sender, EventArgs args)
        {
            if (this.tClient.IsInitialized && this.tClient.IsConnected)
            {
                this.tClient.Disconnect();
            }
        }

        private Task ManageNowStreamingUsers(DiscordClient dClient, PresenceUpdateEventArgs args)
        {
            Task.Run(async () =>
            {
                // Ignore non-SBG events
                if (args.PresenceAfter.Guild.Id != Variables.Guilds.SBG)
                {
                    return;
                }

                DiscordRole streaming = args.PresenceAfter.Guild.GetRole(Roles.SBG.NowStreaming);
                DiscordMember member = await args.PresenceAfter.Guild.GetMemberAsync(args.UserAfter.Id);
                bool wasStreaming = args.PresenceBefore?.Activities.Any(a => a.StreamUrl != null) ?? false;
                DiscordActivity stream = args.PresenceAfter.Activities.Where(a => a.StreamUrl != null).FirstOrDefault();

                // User Started Streaming Intruder
                if (!wasStreaming && stream != null)
                {
                    SearchStreams search = await this.twitchAPI.V5.Search.SearchStreamsAsync(stream.StreamUrl.Replace("https://www.twitch.tv/", string.Empty, StringComparison.Ordinal));

                    foreach (Stream searchStream in search.Streams)
                    {
                        if (searchStream.Game == "Intruder")
                        {
                            await this.activityManager.SetStreamAsync(args.UserAfter.Id, searchStream.Channel.Name.Capitalize(), searchStream.Channel.Url);
                            await member.GrantRoleAsync(streaming);
                            break;
                        }
                    }
                }

                // User Stopped Streaming Intruder
                else if (wasStreaming && stream == null && member.Roles.Contains(streaming))
                {
                    if (this.activityManager.IsStreamOwner(args.UserAfter.Id))
                    {
                        await this.activityManager.ClearStreamAsync();
                    }

                    await member.RevokeRoleAsync(streaming);
                }
            });

            return Task.CompletedTask;
        }
    }
}
