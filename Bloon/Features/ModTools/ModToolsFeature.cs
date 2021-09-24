namespace Bloon.Features.ModTools
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.SlashCommands;

    public class ModToolsFeature : Feature
    {
        private readonly CommandsNextExtension cNext;
        private readonly SlashCommandsExtension slash;

        public ModToolsFeature(DiscordClient dClient)
        {
            this.cNext = dClient.GetCommandsNext();
            this.slash = dClient.GetSlashCommands();
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

            // Disabled due to Zepplein
            //this.cNext.RegisterCommands<MuteUser>();
            //this.cNext.RegisterCommands<KickUser>();
            //this.cNext.RegisterCommands<BanUser>();

            this.slash.RegisterCommands<UserInfoMenu>(Guilds.SBG);
            return base.Enable();
        }
    }
}
