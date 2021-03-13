namespace Bloon.Features.IntruderBackend
{
    using System.Threading.Tasks;
    using Bloon.Commands;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;

    public class StatsCommandFeature : Feature
    {
        private readonly CommandsNextExtension cNext;

        public StatsCommandFeature(DiscordClient dClient)
        {
            this.cNext = dClient.GetCommandsNext();
        }

        public override string Name => "Stats Command";

        public override string Description => "Obtains profile data and ships it off in an embed.";

        public override Task Disable()
        {
            this.cNext.UnregisterCommands<Stats>();

            return base.Disable();
        }

        public override Task Enable()
        {
            this.cNext.RegisterCommands<Stats>();

            return base.Enable();
        }
    }
}
