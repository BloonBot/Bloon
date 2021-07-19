namespace Bloon.Features.SBAInactivity
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables.Channels;
    using DSharpPlus;
    using DSharpPlus.EventArgs;
    using Microsoft.Extensions.DependencyInjection;

    public class SBAInactivityFeature : Feature
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly DiscordClient dClient;
        private readonly JobManager jobManager;
        private readonly SBAInactivityJob sbaInactivityJob;

        public SBAInactivityFeature(IServiceScopeFactory scopeFactory, JobManager jobManager, DiscordClient dClient, BloonLog bloonLog)
        {
            this.scopeFactory = scopeFactory;
            this.dClient = dClient;
            this.jobManager = jobManager;
            this.sbaInactivityJob = new SBAInactivityJob(scopeFactory, dClient, bloonLog);
        }

        public override string Name => "SBA Inactivity Pruning";

        public override string Description => "Responsible for the SBA Pruning. Will not stop tracking message history.";

        public override Task Initialize()
        {
            return base.Initialize();
        }

        public override Task Disable()
        {
            this.jobManager.RemoveJob(this.sbaInactivityJob);

            return base.Disable();
        }

        public override Task Enable()
        {
            this.jobManager.AddJob(this.sbaInactivityJob);

            return base.Enable();
        }
    }
}
