namespace Bloon.Features.RandomResponses
{
    using System.Threading.Tasks;
    using Bloon.Core.Services;
    using DSharpPlus;

    public class RandomResponsesFeature : Feature
    {
        private readonly MessageEvents messageEvents;

        public RandomResponsesFeature(DiscordClient dClient)
        {
            this.messageEvents = new MessageEvents(dClient);
        }

        public override string Name => "Random Responses";

        public override string Description => "Adds a reaction to a message that Clor sometimes says and can 'randomly' unflip/flip tables.";

        public override Task Disable()
        {
            this.messageEvents.Unregister();

            return base.Disable();
        }

        public override Task Enable()
        {
            this.messageEvents.Register();

            return base.Enable();
        }
    }
}
