namespace Bloon.Features.Censor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables.Channels;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using Microsoft.Extensions.DependencyInjection;

    public class CensorFeature : Feature
    {
        private readonly IServiceProvider provider;
        private readonly DiscordClient dClient;
        private readonly CommandsNextExtension cNext;
        private readonly Censorer censorer;

        public CensorFeature(IServiceProvider provider, DiscordClient dClient, Censorer censorer)
        {
            this.provider = provider;
            this.dClient = dClient;
            this.cNext = dClient.GetCommandsNext();
            this.censorer = censorer;
        }

        public override string Name => "Censor";

        public override string Description => "Sends an embed to #bloonside whenever a user triggers a censor pattern.";

        public override Task Disable()
        {
            this.censorer.Reset();
            this.cNext.UnregisterCommands<CensorCommands>();
            this.dClient.MessageCreated -= this.CensorAsync;
            this.dClient.MessageReactionAdded -= this.ProfanityFilterRemove;

            return base.Disable();
        }

        public override Task Enable()
        {
            using IServiceScope scope = this.provider.CreateScope();
            using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            this.censorer.Init(db.Censors.ToList());
            this.cNext.RegisterCommands<CensorCommands>();
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

            DiscordChannel bloonside = await this.dClient.GetChannelAsync(SBGChannels.Bloonside);
            DiscordMessage foulEmbed = await bloonside.GetMessageAsync(args.Message.Id);

            if (args.Message.Channel.Id == SBGChannels.Bloonside && foulEmbed.Content.Contains("Profanity Detected", StringComparison.Ordinal) && foulEmbed.Author.Id == dClient.CurrentUser.Id)
            {
                await foulEmbed.DeleteAsync();
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

            if (this.censorer.TryCensorContent(args.Message.Content, out string censored, out KeyValuePair<int, Regex> censor))
            {
                DiscordChannel sbgMod = args.Message.Channel.Guild.GetChannel(SBGChannels.Bloonside);
                DiscordEmbedBuilder embed = new DiscordEmbedBuilder
                {
                    Color = new DiscordColor(255, 0, 0),
                    Description = $"\"{censored}\"",
                    Timestamp = args.Message.Timestamp,
                    Title = $"**Censor**",
                };
                embed.AddField("Author", args.Message.Author.Mention, true);
                embed.AddField("Pattern", $"`{censor.Value}` (Id: {censor.Key})", true);
                embed.AddField("Original Message", $"[Click Here]({args.Message.JumpLink})", true);

                DiscordMessage embedMessage = await sbgMod.SendMessageAsync(embed);
                await embedMessage.CreateReactionAsync(DiscordEmoji.FromName(this.dClient, ":wastebasket:"));
            }
        }
    }
}
