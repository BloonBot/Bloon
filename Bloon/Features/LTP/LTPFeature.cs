namespace Bloon.Features.LTP
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.SlashCommands;

    public class LTPFeature : Feature
    {
        private readonly CommandsNextExtension cNext;
        private readonly SlashCommandsExtension slash;

        public LTPFeature(DiscordClient dClient)
        {
            this.cNext = dClient.GetCommandsNext();
            this.slash = dClient.GetSlashCommands();
        }

        public override string Name => "LTP Commands";

        public override string Description => "The LTP Commands adds a user to the @Looking to Play role. Disabling will revoke this.";

        public override Task Disable()
        {
            this.cNext.UnregisterCommands<LTPCommands>();

            return base.Disable();
        }

        public override Task Enable()
        {
            this.cNext.RegisterCommands<LTPCommands>();
            this.slash.RegisterCommands<LTPSlashCommand>(Guilds.SBG);

            return base.Enable();
        }
    }
}
