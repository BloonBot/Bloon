namespace Bloon.Features.PackageAccounts
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;

    public class PackageAccountsFeature : Feature
    {
        private readonly CommandsNextExtension cNext;

        public PackageAccountsFeature(DiscordClient dClient)
        {
            this.cNext = dClient.GetCommandsNext();
        }

        public override string Name => "PackageAccounts";

        public override string Description => "Enable/Disable Package Account Creation";

        public override Task Disable()
        {
            this.cNext.UnregisterCommands<AccountsCommands>();

            return base.Disable();
        }

        public override Task Enable()
        {
            this.cNext.RegisterCommands<AccountsCommands>();

            return base.Enable();
        }
    }
}
