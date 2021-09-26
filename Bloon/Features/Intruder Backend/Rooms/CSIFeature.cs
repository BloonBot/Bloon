namespace Bloon.Features.IntruderBackend.Servers
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.SlashCommands;

    public class CSIFeature : Feature
    {
        private readonly CommandsNextExtension cNext;
        private readonly SlashCommandsExtension slash;

        public CSIFeature(DiscordClient dClient)
        {
            this.cNext = dClient.GetCommandsNext();
            this.slash = dClient.GetSlashCommands();
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
            this.slash.RegisterCommands<GSISlashCommand>(Guilds.SBG);
            return base.Enable();
        }
    }
}
