namespace Bloon.Core.Discord
{
    using System;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.Entities;

    public enum LogConsole
    {
        /// <summary>
        /// Unknown channel
        /// </summary>
        None = 0,

        /// <summary>
        /// <see cref="Channels.Bloon.Commands"/>
        /// </summary>
        Commands = 1,

        /// <summary>
        /// <see cref="Channels.Bloon.RoleEdits"/>
        /// </summary>
        RoleEdits = 2,

        /// <summary>
        /// <see cref="Channels.Bloon.SBGUserInfo"/>
        /// </summary>
        UserInfo = 3,
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
                LogConsole.Commands => await this.dClient.GetChannelAsync(Channels.Bloon.Commands),
                LogConsole.RoleEdits => await this.dClient.GetChannelAsync(Channels.Bloon.RoleEdits),
                LogConsole.UserInfo => await this.dClient.GetChannelAsync(Channels.Bloon.SBGUserInfo),
                _ => await this.dClient.GetChannelAsync(Channels.Bloon.ExceptionReporting),
            };
            await channel.SendMessageAsync($"**[{DateTime.UtcNow}]** {DiscordEmoji.FromGuildEmote(this.dClient, emoji)} {message}");
        }

        public async void Error(string message)
        {
            DiscordChannel logChannel = await this.dClient.GetChannelAsync(Channels.Bloon.ExceptionReporting);
            await logChannel.SendMessageAsync(message);
        }
    }
}
