namespace Bloon.Features.IntruderBackend.Servers
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;

    public class CSIFeature : Feature
    {
        private readonly CommandsNextExtension cNext;

        public CSIFeature(DiscordClient dClient)
        {
            this.cNext = dClient.GetCommandsNext();
        }

        public override string Name => "GSI Commands";

        public override string Description => "Allows users to run the servers command which gets room information from the Intruder backend.";

        public override Task Disable()
        {
            this.cNext.UnregisterCommands<ServersCommand>();
            return base.Disable();
        }

        public override Task Enable()
        {
            this.cNext.RegisterCommands<ServersCommand>();

            return base.Enable();
        }
    }
}
