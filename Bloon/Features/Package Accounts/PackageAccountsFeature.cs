namespace Bloon.Features.PackageAccounts
{
    using System;
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;

    public class PackageAccountsFeature : Feature
    {
        private readonly IServiceProvider provider;
        private readonly DiscordClient dClient;

        public PackageAccountsFeature(IServiceProvider provider, DiscordClient dClient)
        {
            this.provider = provider;
            this.dClient = dClient;
        }

        public override string Name => "PackageAccounts";

        public override string Description => "Enable/Disable Package Account Creation";

        public override Task Disable()
        {

            return base.Disable();
        }

        public override Task Enable()
        {
            this.dClient.GuildMemberUpdated += DClient_GuildMemberUpdated;
            return base.Enable();
        }

        private Task DClient_GuildMemberUpdated(DiscordClient sender, DSharpPlus.EventArgs.GuildMemberUpdateEventArgs e)
        {
            // something
            return Task.CompletedTask;
        }
    }
}
