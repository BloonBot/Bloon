namespace Bloon.Features.Censor
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Bloon.Core.Services;
    using Bloon.Variables.Channels;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;

    public class CensorFeature : Feature
    {
        private readonly DiscordClient dClient;

        private Censor censor;

        public CensorFeature(DiscordClient dClient)
        {
            this.dClient = dClient;
        }

        public override string Name => "Profanity Filter Embeds";

        public override string Description => "Sends an embed to #aug whenever a user triggers the naughtywordlist.";

        public override Task Disable()
        {
            this.dClient.MessageCreated -= this.CensorAsync;
            this.dClient.MessageReactionAdded -= this.ProfanityFilterRemove;

            return base.Disable();
        }

        public override Task Enable()
        {
            this.censor = new Censor(File.ReadAllLines(Directory.GetCurrentDirectory() + "/Features/Censor/naughtywords.txt", Encoding.UTF8));
            this.dClient.MessageCreated += this.CensorAsync;
            this.dClient.MessageReactionAdded += this.ProfanityFilterRemove;

            return base.Enable();
        }

        private async Task ProfanityFilterRemove(DiscordClient dClient, MessageReactionAddEventArgs args)
        {
            if (args.User.Id == dClient.CurrentUser.Id || (args.Channel.Id != SBGChannels.Bloonside && !args.Emoji.Equals(DiscordEmoji.FromName(this.dClient, ":wastebasket:"))))
            {
                return;
            }

            DiscordChannel aug = await this.dClient.GetChannelAsync(SBGChannels.Bloonside).ConfigureAwait(false);
            DiscordMessage foulEmbed = await aug.GetMessageAsync(args.Message.Id).ConfigureAwait(false);

            if (args.Message.Channel.Id == SBGChannels.Bloonside && foulEmbed.Content.Contains("Profanity Detected", StringComparison.Ordinal) && foulEmbed.Author.Id == dClient.CurrentUser.Id)
            {
                await foulEmbed.DeleteAsync().ConfigureAwait(false);
            }

            return;
        }

        /// <summary>
        /// This will detect if a message within the SBG guild contains a word that is stored in the naughtywords file.
        /// </summary>
        /// <param name="args">Message arguments.</param>
        /// <returns>Profantiy warning to AUG chat.</returns>
        private async Task CensorAsync(DiscordClient dClient, MessageCreateEventArgs args)
        {
            if (args.Author.IsBot)
            {
                return;
            }

            if (this.censor.HasNaughtyWord(args.Message.Content))
            {
                DiscordChannel sbgAUG = args.Message.Channel.Guild.GetChannel(SBGChannels.Bloonside);
                DiscordEmbed foul = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor(255, 0, 0),
                    Timestamp = args.Message.Timestamp,
                    Title = $"**Profanity Detected**",
                    Description = $"\"{args.Message.Content}\"\n\nby {args.Message.Author.Username}\n[View Message]({args.Message.JumpLink})",
                };

                DiscordMessage embedMessage = await sbgAUG.SendMessageAsync("Profanity Detected", embed: foul).ConfigureAwait(false);
                await embedMessage.CreateReactionAsync(DiscordEmoji.FromName(this.dClient, ":wastebasket:")).ConfigureAwait(false);
            }
        }
    }
}
