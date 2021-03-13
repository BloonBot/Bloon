namespace Bloon.Core.Discord
{
    using System;
    using Bloon.Variables.Channels;
    using DSharpPlus;
    using DSharpPlus.Entities;

    public enum LogConsole
    {
        /// <summary>
        /// Unknown channel
        /// </summary>
        None = 0,

        /// <summary>
        /// <see cref="BloonChannels.Console"/>
        /// </summary>
        Console = 1,

        /// <summary>
        /// <see cref="BloonChannels.Commands"/>
        /// </summary>
        Commands = 2,

        /// <summary>
        /// <see cref="BloonChannels.Jobs"/>
        /// </summary>
        Jobs = 3,

        /// <summary>
        /// <see cref="BloonChannels.RoleEdits"/>
        /// </summary>
        RoleEdits = 4,

        /// <summary>
        /// <see cref="BloonChannels.SBGUserInfo"/>
        /// </summary>
        UserInfo = 5,
    }

    public class BloonLog
    {
        private readonly DiscordClient dClient;

        public BloonLog(DiscordClient dClient)
        {
            this.dClient = dClient;
        }

        public async void Information(LogConsole consoleChannel, ulong emoji, string message)
        {
            DiscordChannel channel = consoleChannel switch
            {
                LogConsole.Commands => await this.dClient.GetChannelAsync(BloonChannels.Commands).ConfigureAwait(false),
                LogConsole.Jobs => await this.dClient.GetChannelAsync(BloonChannels.Jobs).ConfigureAwait(false),
                LogConsole.RoleEdits => await this.dClient.GetChannelAsync(BloonChannels.RoleEdits).ConfigureAwait(false),
                LogConsole.UserInfo => await this.dClient.GetChannelAsync(BloonChannels.SBGUserInfo).ConfigureAwait(false),
                _ => await this.dClient.GetChannelAsync(BloonChannels.Console).ConfigureAwait(false),
            };
            await channel.SendMessageAsync($"**[{DateTime.UtcNow}]** {DiscordEmoji.FromGuildEmote(this.dClient, emoji)} {message}").ConfigureAwait(false);
        }

        public async void Error(string message)
        {
            DiscordChannel logChannel = await this.dClient.GetChannelAsync(BloonChannels.ExceptionReporting).ConfigureAwait(false);
            await logChannel.SendMessageAsync(message).ConfigureAwait(false);
        }
    }
}
