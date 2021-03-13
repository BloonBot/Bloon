namespace Bloon.Features.Spam
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Core.Services;
    using Bloon.Variables.Channels;
    using DSharpPlus;
    using DSharpPlus.EventArgs;
    using Microsoft.Extensions.DependencyInjection;

    public class SpamFeature : Feature
    {
        private const int AdvertThreshold = 200;

        private readonly IServiceScopeFactory scopeFactory;
        private readonly DiscordClient dClient;

        private int messageCounter;

        public SpamFeature(IServiceScopeFactory scopeFactory, DiscordClient dClient)
        {
            this.scopeFactory = scopeFactory;
            this.dClient = dClient;
        }

        public override string Name => "Totally Not Spam";

        public override string Description => "After X messages, the bot will post a 'random' message within #intruder-general.\n Message count only applies for messages within #intruder-general";

        public override Task Disable()
        {
            this.dClient.MessageCreated -= this.GeneralSpamAsync;

            return base.Disable();
        }

        public override Task Enable()
        {
            this.dClient.MessageCreated += this.GeneralSpamAsync;

            return base.Enable();
        }

        /// <summary>
        /// Increments messageNum.
        /// If greater than/equal to <see cref="AdvertThreshold"/>, send random (advertisement) message from the DB.
        /// </summary>
        /// <param name="args">Message arguments.</param>
        /// <returns>Task.</returns>
        private async Task GeneralSpamAsync(DiscordClient dClient, MessageCreateEventArgs args)
        {
            if (args.Author.IsBot || args.Channel.Id != SBGChannels.General)
            {
                return;
            }

            this.messageCounter += 1;

            if (this.messageCounter >= AdvertThreshold)
            {
                Random rnd = new Random();

                using IServiceScope scope = this.scopeFactory.CreateScope();
                using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();

                int messageCount = db.SpamMessages.Count();
                int random = rnd.Next(0, messageCount);

                SpamMessage randomMessage = db.SpamMessages.Skip(random).FirstOrDefault();
                this.messageCounter = 0;

                await args.Channel.SendMessageAsync(randomMessage.Value).ConfigureAwait(false);
            }
        }
    }
}
