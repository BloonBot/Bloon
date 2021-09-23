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
        private readonly DiscordClient dClient;
        private readonly CommandsNextExtension cNext;
        private readonly SlashCommandsExtension slash;

        public LTPFeature(DiscordClient dClient)
        {
            this.dClient = dClient;
            this.cNext = dClient.GetCommandsNext();
            this.slash = dClient.GetSlashCommands();
        }

        public override string Name => "LTP Commands";

        public override string Description => "The LTP Commands adds a user to the @Looking to Play role. Disabling will revoke this.";

        public override Task Initialize()
        {
            this.dClient.Ready += this.OnReady;

            return base.Initialize();
        }

        public override Task Disable()
        {
            this.cNext.UnregisterCommands<LTPCommands>();

            return base.Disable();
        }

        public override Task Enable()
        {
            this.cNext.RegisterCommands<LTPCommands>();

            return base.Enable();
        }

        private Task OnReady(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs e)
        {
            this.slash.RegisterCommands<LTPSlashCommand>(Guilds.SBG);

            return Task.CompletedTask;
        }
    }
}
