namespace Bloon.Core.Commands
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Bloon.Core.Commands.Attributes;
    using Bloon.Core.Database;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.SlashCommands;

    [SlashLimitedChannels]
    public class GeneralSlashCommands : ApplicationCommandModule
    {
        [SlashCommand("ping", "Check the bot's ping to the Discord gateway.")]
        public Task PingPongAsync(InteractionContext ctx)
        {
            return ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"pong! Latency: {ctx.Client.Ping}ms").AsEphemeral(true));
        }

        [SlashCommand("info", "Responds with a message containing lots of useful links and information for getting around the community.")]
        public async Task InfoAsync(InteractionContext ctx)
        {
            DiscordEmbedBuilder linksEmbed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = $"༼ つ ◕_◕ ༽つ GIBE LINKS",
                },
                Color = new DiscordColor(95, 95, 95),
                Timestamp = DateTime.UtcNow,
                Title = $"**Important Links**",
            };

            StringBuilder socialLinks = new StringBuilder();

            socialLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Platform.Reddit)} | [**Reddit**](https://www.reddit.com/r/Intruder)\n");
            socialLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Platform.Twitch)} | [**Twitch**](https://www.twitch.tv/superbossgames)\n");
            socialLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Platform.Twitter)} | [**Twitter**](https://twitter.com/SuperbossGames/)\n");
            socialLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Platform.YouTube)} | [**Youtube**](https://www.youtube.com/superbossgames)\n");

            StringBuilder steamLinks = new StringBuilder();
            steamLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Platform.Steam)} | [**Steam Store Page**](https://store.steampowered.com/app/518150/Intruder/)\n");
            steamLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Platform.Steam)} | [**Intruder Steam Workshop**](https://steamcommunity.com/app/518150/workshop/)\n");
            steamLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Platform.Steam)} | [**Steam Forums**](https://steamcommunity.com/app/518150/discussions/)\n");

            StringBuilder intruderLinks = new StringBuilder();
            intruderLinks.Append($"{DiscordEmoji.FromName(ctx.Client, ":link:")} | [**Website**](https://intruderfps.com/)\n");
            intruderLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Server.Players)} | [**Agents**](https://intruderfps.com/agents)\n");
            intruderLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Server.Map)} | [**Roadmap**](https://intruderfps.com/roadmap)\n");
            intruderLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Command.Run)} | [**Trello**](https://trello.com/b/ebmvvFVE/intruder-board)\n");
            intruderLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Platform.Wiki)} | [**Wiki**](https://wiki.bloon.info/)\n");
            intruderLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Platform.Wiki)} | [**TMC**](https://wiki.bloon.info/index.php?title=TMC)\n");

            StringBuilder botLinks = new StringBuilder();
            botLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Platform.Discord)} | [**Bloon Dev Discord**](https://discord.gg/tAVydGr)\n");
            botLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Platform.Github)} | [**GitHub Repo**](https://github.com/BloonBot/Bloon)\n");
            botLinks.Append($"{DiscordEmoji.FromName(ctx.Client, ":dollar:")} | [**Donate**](https://www.patreon.com/bloon)\n");

            StringBuilder discordLinks = new StringBuilder();
            discordLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Platform.Discord)} | [**Discord Guidelines**](https://discord.com/guidelines)\n");
            discordLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Platform.Discord)} | [**Website**](https://discord.com/)\n");
            discordLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Platform.Discord)} | [**Server Invite**](https://discord.gg/superbossgames)\n");

            linksEmbed.AddField($"Social Links", socialLinks.ToString(), true);
            linksEmbed.AddField($"Steam Links", steamLinks.ToString(), true);
            linksEmbed.AddField($"Intruder", intruderLinks.ToString(), true);
            linksEmbed.AddField($"Bot Links", botLinks.ToString(), true);
            linksEmbed.AddField($"Discord Links", discordLinks.ToString(), true);

            string extensions = $"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Browser.Chrome)} [**Chrome**](https://chrome.google.com/webstore/detail/intruder-notifications/aoebpknpfcepopfgnbnikaipjeekalim) | "
                + $"[**Firefox**](https://addons.mozilla.org/en-US/firefox/addon/intruder-notifications/) {DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Browser.Firefox)}";
            linksEmbed.AddField("Browser Extensions", extensions, true);

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(linksEmbed.Build()).AsEphemeral(true));
        }
    }
}
