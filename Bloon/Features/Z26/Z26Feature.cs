namespace Bloon.Features.Z26
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;

    public class Z26Feature : Feature
    {
        private readonly CommandsNextExtension cNext;

        public Z26Feature(DiscordClient dClient)
        {
            this.cNext = dClient.GetCommandsNext();
        }

        public override string Name => "Z26";

        public override string Description => "Z26 sucks and so does the dude who made it.";

        public override Task Disable()
        {
            //this.cNext.UnregisterCommands<Z26Commands>();

            return base.Disable();
        }

        public override Task Enable()
        {
            //this.cNext.RegisterCommands<Z26Commands>();

            return base.Enable();
        }
    }
}
