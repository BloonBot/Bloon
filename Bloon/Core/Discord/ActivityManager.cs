namespace Bloon.Core.Discord
{
    using System;
    using System.Net.WebSockets;
    using System.Threading.Tasks;
    using Bloon.Variables.Channels;
    using Bloon.Variables.Emojis;
    using DSharpPlus;
    using DSharpPlus.Entities;

    /// <summary>
    /// Manages Bloon's activity and streaming statuses.
    /// </summary>
    public class ActivityManager
    {
        private const int AutoResetMs = 3000;
        private const string DefaultActivity = "you through my camera";

        private readonly DiscordClient dClient;

        private ulong streamOwnerID;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityManager"/> class.
        /// </summary>
        /// <param name="dClient">Discord socket client.</param>
        public ActivityManager(DiscordClient dClient)
        {
            this.dClient = dClient;
        }

        /// <summary>
        /// Clears the stream and returns to <see cref="DefaultActivity"/>.
        /// </summary>
        /// <returns>Awaitable task.</returns>
        public async Task ClearStreamAsync()
        {
            this.streamOwnerID = 0;
            await this.ResetActivityAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Checks whether a user is the current stream's owner.
        /// </summary>
        /// <param name="streamerID">Discord user id.</param>
        /// <returns>Boolean.</returns>
        public bool IsStreamOwner(ulong streamerID) => this.streamOwnerID == streamerID;

        /// <summary>
        /// Resets Bloon's activity to <see cref="DefaultActivity"/>.
        /// </summary>
        /// <returns>Awaitable task.</returns>
        public Task ResetActivityAsync() => this.TrySetActivityAsync(DefaultActivity, ActivityType.Watching);

        /// <summary>
        /// Advertises/Shares a stream. '<paramref name="force"/>' overrides any current stream.
        /// </summary>
        /// <param name="streamerID">Discord user id.</param>
        /// <param name="streamerName">Stream name.</param>
        /// <param name="url">Stream url.</param>
        /// <param name="force">Force Bloon to switch to this stream.</param>
        /// <returns>Awaitable task.</returns>
#pragma warning disable CA1054 // Uri parameters should not be strings
        public async Task SetStreamAsync(ulong streamerID, string streamerName, string url, bool force = false)
#pragma warning restore CA1054 // Uri parameters should not be strings
        {
            if (!force && this.dClient.CurrentUser.Presence.Activity.ActivityType == ActivityType.Streaming)
            {
                return;
            }

            DiscordChannel sbgGeneral = await this.dClient.GetChannelAsync(SBGChannels.General).ConfigureAwait(false);
            DiscordEmbed streamEmbed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = url,
                    IconUrl = DiscordEmoji.FromGuildEmote(this.dClient, PlatformEmojis.Twitch).Url,
                },
                Description = $"{streamerName} is streaming Intruder! Stop by and check them out! [**{streamerName}'s Stream**]({url})",
                Color = new DiscordColor(100, 64, 165),
                Timestamp = DateTime.UtcNow,
                Title = $"*Stream Detected!*",
            };

            this.streamOwnerID = streamerID;
            await this.dClient.UpdateStatusAsync(new DiscordActivity
            {
                ActivityType = ActivityType.Streaming,
                Name = $"Intruder with {streamerName}",
                StreamUrl = url,
            }).ConfigureAwait(false);

            await sbgGeneral.SendMessageAsync(embed: streamEmbed).ConfigureAwait(false);
        }

        /// <summary>
        /// Sets Bloon's activity if not currently streaming.
        /// </summary>
        /// <param name="activity">Activity description.</param>
        /// <param name="activityType">Activity type.</param>
        /// <param name="autoReset">Automatically switch back to <see cref="DefaultActivity"/> after <see cref="AutoResetMs"/>.</param>
        /// <returns>Awaitable task.</returns>
        public async Task TrySetActivityAsync(string activity, ActivityType activityType, bool autoReset = false)
        {
            if ((this.dClient.CurrentUser.Presence.Activity.ActivityType == ActivityType.Streaming && activity != DefaultActivity)
                || Bot.SocketState != WebSocketState.Open)
            {
                return;
            }

            await this.dClient.UpdateStatusAsync(new DiscordActivity
            {
                ActivityType = activityType,
                Name = activity,
            }).ConfigureAwait(false);

            if (autoReset)
            {
                await Task.Delay(AutoResetMs).ConfigureAwait(false);
                await this.ResetActivityAsync().ConfigureAwait(false);
            }
        }
    }
}
