namespace Bloon.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Variables;
    using Bloon.Variables.Channels;
    using Bloon.Variables.Emojis;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;

    public class FeatureControlEvents : Feature
    {
        private readonly DiscordClient dClient;
        private readonly FeatureManager featureManager;

        public FeatureControlEvents(DiscordClient dClient, FeatureManager featureManager)
        {
            this.dClient = dClient;
            this.featureManager = featureManager;
        }

        public override string Name => "FeatureControl";

        public override string Description => "Feature controls using embeds";

        public override bool Protected => true;

        public override Task Enable()
        {
            this.dClient.GuildAvailable += this.OnGuildAvailable;
            this.dClient.MessageReactionAdded += this.OnMessageReactionAdded;

            return base.Enable();
        }

        private static DiscordEmbed CreateFeatureEmbed(Feature feature)
        {
            return new DiscordEmbedBuilder
            {
                Title = feature.Name,
                Description = feature.Description,
                Timestamp = DateTime.UtcNow,
                Color = feature.Enabled ? new DiscordColor(21, 137, 255) : new DiscordColor(131, 126, 124),
            };
        }

        // Ignore timestamps
        private static bool IdenticalEmbed(DiscordEmbed a, DiscordEmbed b)
        {
            return a.Title == b.Title
                && a.Description == b.Description
                && a.Color == b.Color;
        }

        private async Task OnMessageReactionAdded(DiscordClient dClient, MessageReactionAddEventArgs args)
        {
            if (args.Guild.Id != Guilds.Bloon || args.Channel.Id != BloonChannels.Settings || args.User.Id == dClient.CurrentUser.Id)
            {
                return;
            }

            DiscordChannel settingsChannel = await this.dClient.GetChannelAsync(BloonChannels.Settings);
            DiscordChannel botMods = await this.dClient.GetChannelAsync(BloonChannels.BotMods);
            DiscordMessage featureMessage = await settingsChannel.GetMessageAsync(args.Message.Id);
            Feature feature = this.featureManager.Features.Where(f => f.Name == featureMessage.Embeds[0]?.Title).FirstOrDefault();

            if (feature == null)
            {
                return;
            }
            else if (args.Emoji.Id == FeatureEmojis.ToggleOff && feature.Enabled)
            {
                await feature.Disable();
                await this.featureManager.UpdateFeatureStatusAsync(feature.Name, false);
                await botMods.SendMessageAsync($"{args.User.Username}#{args.User.Discriminator} *disabled* `{feature.Name}` at {DateTime.Now}\n" +
                    $"{featureMessage.JumpLink}");
            }
            else if (args.Emoji.Id == FeatureEmojis.ToggleOn && !feature.Enabled)
            {
                await feature.Enable();
                await this.featureManager.UpdateFeatureStatusAsync(feature.Name, true);
                await botMods.SendMessageAsync($"{args.User.Username}#{args.User.Discriminator} *enabled* `{feature.Name}` at {DateTime.Now}\n" +
                    $"{featureMessage.JumpLink}");
            }

            await featureMessage.ModifyAsync(embed: CreateFeatureEmbed(feature));
            await featureMessage.DeleteReactionAsync(args.Emoji, args.User);
        }

        private async Task OnGuildAvailable(DiscordClient dClient, GuildCreateEventArgs args)
        {
            if (args.Guild.Id != Guilds.Bloon)
            {
                return;
            }

            await this.dClient.Guilds[Guilds.Bloon].GetEmojisAsync();

            DiscordChannel settingsChannel = await this.dClient.GetChannelAsync(BloonChannels.Settings);
            IReadOnlyList<DiscordMessage> messages = await settingsChannel.GetMessagesAsync(this.featureManager.Features.Count);

            for (int i = 0; i < this.featureManager.Features.Count; i++)
            {
                Feature feature = this.featureManager.Features[i];
                DiscordMessage message = messages.Where(m => m.Embeds[0]?.Title == feature.Name).FirstOrDefault();
                DiscordEmbed newEmbed = CreateFeatureEmbed(feature);

                if (message == null)
                {
                    message = await settingsChannel.SendMessageAsync(embed: newEmbed);
                    await message.CreateReactionAsync(DiscordEmoji.FromGuildEmote(this.dClient, FeatureEmojis.ToggleOff));
                    await message.CreateReactionAsync(DiscordEmoji.FromGuildEmote(this.dClient, FeatureEmojis.ToggleOn));
                }
                else if (!IdenticalEmbed(message.Embeds[0], newEmbed))
                {
                    await message.ModifyAsync(embed: newEmbed);
                }
            }
        }
    }
}
