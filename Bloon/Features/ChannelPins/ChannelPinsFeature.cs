namespace Bloon.Features.ChannelPins
{
    using System.Threading.Tasks;
    using Bloon.Core.Services;
    using DSharpPlus;
    using DSharpPlus.EventArgs;

    public class ChannelPinsFeature : Feature
    {
        private const int PinWarningThreshold = 45;

        private readonly DiscordClient dClient;

        public ChannelPinsFeature(DiscordClient dClient)
        {
            this.dClient = dClient;
        }

        public override string Name => "Channel Pin Limit Warning";

        public override string Description => "Sends a warning message to the channel whenever pin limit is about to be reached./n This is triggered whenever someone adds or removes a pin.";

        public override Task Disable()
        {
            this.dClient.ChannelPinsUpdated -= this.OnChannelPinsUpdatedAsync;

            return base.Disable();
        }

        public override Task Enable()
        {
            this.dClient.ChannelPinsUpdated += this.OnChannelPinsUpdatedAsync;

            return base.Enable();
        }

        /// <summary>
        /// Checks channel's number of pinned posts and responds with a warning if close to 50 pins.
        /// Warning threshold is defined in <see cref="PinWarningThreshold"/>.
        /// </summary>
        /// <param name="args">Event args.</param>
        /// <returns>Task.</returns>
        private async Task OnChannelPinsUpdatedAsync(DiscordClient dClient, ChannelPinsUpdateEventArgs args)
        {
            int pinCount = (await args.Channel.GetPinnedMessagesAsync().ConfigureAwait(false)).Count;
            if (pinCount >= PinWarningThreshold)
            {
                await args.Channel.SendMessageAsync($"Approaching the pinned message limit [{pinCount}/50]!").ConfigureAwait(false);
            }
        }
    }
}
