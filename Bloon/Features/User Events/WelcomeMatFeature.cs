namespace Bloon.Features.Doorman
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables.Channels;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;

    public class WelcomeMatFeature : Feature
    {
        private readonly string[] greetings =
        {
            "Здравствуйте",
            "Добрый день",
            "नमस्ते",
            "你好",
            "Aloha",
            "Bonjour",
            "Ciao",
            "Hey",
            "Hello",
            "Herzlich willkommen",
            "Hi",
            "Hola",
            "G'day",
            "Good to see you",
            "Greetings",
            "How do you do",
            "How's it going",
            "Konnichi wa",
            "Nice to meet you",
            "Olá",
            "Pleased to meet you",
            "Sup",
            "Wassup",
            "Welcome",
            "What's up",
            "Whazzup",
            "Yo",
            "01101000 01100101 01101100 01101100 01101111",
            "Ayeeee",
            "We've been expecting you",
            "Cheers!",
            "It's dangerous to go alone",
            "Brace yourselves",
            "Holy crap its you",
            "Make yourself at home",
            "AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH",
            "Make yourself at home",
            "Don't blame me",
            "Not all of us our experts",
            "Whoop! There they are",
            "Make Z26 proud",
            "Aye bb",
            "Whats good in the world",
            "Look who decided to show up",
            "Что случилось",
            "Прикосновение ноги Роба",
            "Пожалуйста, подумайте о покупке игры",
            "Ubuden gæst byder dig velkommen",
            "Hej! Der er du",
            "Bonne jour née à vous",
            "Les escargots salés sont mon deuxième aliment préféré",
            "Entfernen Sie diesen Herzog nicht",
            "ようこそ",
            "환영",
            "Nie pop mnie",
            "O bate-papo da equipe é para perdedores",
            "Ha llegado el momento",
            "$20 הוא השבט הטוב ביותר",
        };

        private readonly DiscordClient dClient;
        private readonly BloonLog bloonLog;

        public WelcomeMatFeature(DiscordClient dClient, BloonLog bloonLog)
        {
            this.dClient = dClient;
            this.bloonLog = bloonLog;
        }

        public override string Name => "Welcome Mat";

        public override string Description => "Welcomes a user with a random phrase.";

        public override Task Disable()
        {
            this.dClient.GuildMemberAdded -= this.OnGuildMemberAddedAsync;

            return base.Disable();
        }

        public override Task Enable()
        {
            this.dClient.GuildMemberAdded += this.OnGuildMemberAddedAsync;

            return base.Enable();
        }

        private async Task OnGuildMemberAddedAsync(DiscordClient dClient, GuildMemberAddEventArgs args)
        {
            DiscordChannel sbgChannel = await this.dClient.GetChannelAsync(SBGChannels.Bloonside).ConfigureAwait(false);
            DiscordColor colorDate = new DiscordColor(95, 95, 95);

            // If guild isn't SBG, just ignore this user join event.
            if (args.Guild.Id != Variables.Guilds.SBG)
            {
                return;
            }

            // If the user's account age is less than 24 hours, we may be dealing with a throwaway/spam/etc. account.
            // Change their welcome embed color to flag these users as they may be malicious
            if ((DateTime.UtcNow - args.Member.CreationTimestamp.UtcDateTime).TotalHours <= 24)
            {
                colorDate = new DiscordColor(249, 183, 255);
            }

            Random random = new Random();
            string greeting = this.greetings[random.Next(0, this.greetings.Length)];

            DiscordEmbed userDetails = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    Text = $"Account Created: {args.Member.CreationTimestamp.UtcDateTime.ToString("D", CultureInfo.InvariantCulture)}",
                },
                Color = colorDate,
                Timestamp = DateTime.UtcNow,
                Title = $"**New User Joined** | {args.Member.DisplayName}",
                Description = $"**User**: {args.Member.Username}#{args.Member.Discriminator}\n" +
                    $"**ID**: {args.Member.Id}\n",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = args.Member.AvatarUrl,
                },
            };

            await sbgChannel.SendMessageAsync(string.Empty, embed: userDetails).ConfigureAwait(false);
            sbgChannel = await this.dClient.GetChannelAsync(SBGChannels.General).ConfigureAwait(false);

            await sbgChannel.SendMessageAsync(string.Empty, embed: userDetails).ConfigureAwait(false);
        }
    }
}
