namespace Bloon.Features.Z26
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.SlashCommands;

    public class Z26Feature : Feature
    {
        private readonly SlashCommandsExtension slash;

        public Z26Feature(DiscordClient dClient)
        {
            this.slash = dClient.GetSlashCommands();
        }

        public override string Name => "Z26";

        public override string Description => "Z26 sucks and so does the dude who made it.";

        public override Task Disable()
        {
            return base.Disable();
        }

        public override Task Enable()
        {
            //this.slash.RegisterCommands<Z26Commands>(Guilds.Bloon);

            return base.Enable();
        }
    }
}
