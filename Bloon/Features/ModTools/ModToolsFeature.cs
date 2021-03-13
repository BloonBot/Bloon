namespace Bloon.Features.ModTools
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;

    public class ModToolsFeature : Feature
    {
        private readonly CommandsNextExtension cNext;

        public ModToolsFeature(DiscordClient dClient)
        {
            this.cNext = dClient.GetCommandsNext();
        }

        public override string Name => "Mod Tools";

        public override string Description => "Special commands for SBG moderators.";

        public override Task Disable()
        {
            this.cNext.UnregisterCommands<AgentSearch>();

            return base.Disable();
        }

        public override Task Enable()
        {
            this.cNext.RegisterCommands<AgentSearch>();
            this.cNext.RegisterCommands<UserInfo>();
            return base.Enable();
        }
    }
}
