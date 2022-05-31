#pragma warning disable CA1822 // Mark members as static
namespace Bloon.Core.Commands
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Bloon.Core.Commands.Attributes;
    using Bloon.Variables;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    /// <summary>
    /// This class pertains to the commands that can be ran practically anywhere. They're typically short and simple commands to run.
    /// </summary>
    [ModuleLifespan(ModuleLifespan.Transient)]
    [LimitedChannels]
    public class GeneralCommands : BaseCommandModule
    {
        private readonly Dictionary<char, char> vecishMapping = new Dictionary<char, char>()
        {
            { 'Q', 'W' },
            { 'W', 'E' },
            { 'E', 'R' },
            { 'R', 'T' },
            { 'T', 'Y' },
            { 'Y', 'U' },
            { 'U', 'I' },
            { 'I', 'O' },
            { 'O', 'P' },
            { 'P', 'Q' },
            { 'A', 'S' },
            { 'S', 'D' },
            { 'D', 'F' },
            { 'F', 'G' },
            { 'G', 'H' },
            { 'H', 'J' },
            { 'J', 'K' },
            { 'K', 'L' },
            { 'L', 'A' },
            { 'Z', 'X' },
            { 'X', 'C' },
            { 'C', 'V' },
            { 'V', 'B' },
            { 'B', 'N' },
            { 'N', 'M' },
            { 'M', 'Z' },
        };

        [Command("ping")]
        [Description("This command is to be used when you think the bot is frozen or stuck. It'll reply with **pong**")]
        public Task PingPongAsync(CommandContext ctx)
        {
            return ctx.RespondAsync($"pong! Latency: {ctx.Client.Ping}ms");
        }

        [Command("dev")]
        [Description("Shows the invite link to my personal server")]
        public Task DevelopmentURLAsync(CommandContext ctx)
        {
            return ctx.RespondAsync("https://discord.gg/tAVydGr");
        }

        [Command("github")]
        [Aliases("repo", "repository")]
        [Description("Shows the link to the GitHub repository")]
        public Task GitHubURLAsync(CommandContext ctx)
        {
            return ctx.RespondAsync("https://github.com/BloonBot/Bloon");
        }

        [Command("trello")]
        [Description("Shows the trello feature board link")]
        public Task TrelloUrlAsync(CommandContext ctx)
        {
            return ctx.RespondAsync("https://trello.com/b/ebmvvFVE/intruder-board");
        }

        [Command("bugs")]
        [Aliases("suggestions", "suggest", "bug", "ideas", "helprace")]
        [Description("Shows the helprace bug tracking link")]
        public Task BugsUrlAsync(CommandContext ctx)
        {
            return ctx.RespondAsync("https://superbossgames.helprace.com/");
        }

        [Command("donate")]
        [Description("Something something pizza and beer")]
        public async Task DonationUrlAsync(CommandContext ctx)
        {
            await (await ctx.Member.CreateDmChannelAsync()).SendMessageAsync("https://www.patreon.com/bloon");
        }

        [Command("extensions")]
        [Aliases("addons", "plugins", "browsers", "chrome", "chromesucks", "firefox", "firefoxbestbrowser")]
        [Description("Shows the available browser extensions")]
        public Task ExtensionsAsync(CommandContext ctx)
        {
            return ctx.RespondAsync(
                $"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Browser.Chrome)} <https://chrome.google.com/webstore/detail/intruder-notifications/aoebpknpfcepopfgnbnikaipjeekalim>\n" +
                $"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Browser.Firefox)} <https://addons.mozilla.org/en-US/firefox/addon/intruder-notifications/>");
        }

        [Command("vecish")]
        [Description("Shift a message using the American keyboard layout")]
        [Hidden]
        public async Task VecishAsync(CommandContext ctx, [RemainingText] string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                await ctx.Channel.SendMessageAsync("Try again: `.vecish <the string you want converted which you would have added had you looked up how to use this command>`");
                return;
            }

            char[] chars = message.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                char letter = chars[i];

                // Ignore anything that isn't a letter
                if (!char.IsLetter(letter))
                {
                    chars[i] = letter;
                    continue;
                }

                // Do the vecish thing
                chars[i] = this.vecishMapping[char.ToUpperInvariant(letter)];

                if (char.IsLower(letter))
                {
                    chars[i] = char.ToLowerInvariant(chars[i]);
                }
            }

            // await ctx.RespondAsync(new string(chars));
        }
    }
}
#pragma warning restore CA1822 // Mark members as static
