namespace Bloon.Features.TwitchMarley
{
    using System.Collections.Generic;
    using System.Linq;
    using Bloon.Core.Discord;
    using TwitchLib.Api;
    using TwitchLib.Api.Helix.Models.Streams.GetStreams;
    using TwitchLib.Api.Services;
    using TwitchLib.Api.Services.Events.LiveStreamMonitor;
    using TwitchLib.Api.V5.Models.Channels;

    public class StreamEvents
    {
        private readonly ActivityManager activityManager;
        private readonly TwitchAPI twitchAPI;
        private readonly string twitchChannel;

        private LiveStreamMonitorService monitor;

        public StreamEvents(ActivityManager activityManager, TwitchAPI twitchAPI)
        {
            this.activityManager = activityManager;
            this.twitchAPI = twitchAPI;
            this.twitchChannel = "Superbossgames";
        }

        public async void Register()
        {
            if (this.monitor == null)
            {
                this.monitor = new LiveStreamMonitorService(this.twitchAPI);

                List<string> channelIDs = (await this.twitchAPI.Helix.Users.GetUsersAsync(logins: new List<string> { this.twitchChannel }))
                    .Users
                    .Select(x => x.Id)
                    .ToList();

                this.monitor.SetChannelsById(channelIDs);

                this.monitor.OnStreamOnline += this.OnStreamOnline;
                this.monitor.OnStreamOffline += this.OnStreamOffline;
                this.monitor.OnStreamUpdate += this.OnStreamUpdate;
            }

            this.monitor.Start();
        }

        public void Unregister()
        {
            this.monitor.Stop();
        }

        private async void OnStreamOffline(object sender, OnStreamOfflineArgs args)
        {
            await this.activityManager.ClearStreamAsync();
        }

        private void OnStreamOnline(object sender, OnStreamOnlineArgs args)
        {
            this.OnStreamOnlineOrUpdate(args.Channel, args.Stream);
        }

        private void OnStreamUpdate(object sender, OnStreamUpdateArgs args)
        {
            this.OnStreamOnlineOrUpdate(args.Channel, args.Stream);
        }

        private async void OnStreamOnlineOrUpdate(string channelID, Stream stream)
        {
            Channel channel = await this.twitchAPI.V5.Channels.GetChannelByIDAsync(channelID);
            await this.activityManager.SetStreamAsync(1, stream.Title, channel.Url, true);
        }
    }
}
