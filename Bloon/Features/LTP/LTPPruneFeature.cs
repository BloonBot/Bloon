namespace Bloon.Features.LTP
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using DSharpPlus;
    using Microsoft.Extensions.DependencyInjection;

    public class LTPPruneFeature : Feature
    {
        private readonly JobManager jobManager;
        private readonly LTPPruneJob ltpPruneJob;

        public LTPPruneFeature(IServiceScopeFactory scopeFactory, JobManager jobManager, DiscordClient dClient, BloonLog bloonLog)
        {
            this.jobManager = jobManager;
            this.ltpPruneJob = new LTPPruneJob(scopeFactory, dClient, bloonLog);
        }

        public override string Name => "LTP Pruning";

        public override string Description => "Prune @Looking To Play after 7 days after not playing Intruder.";

        public override Task Disable()
        {
            this.jobManager.RemoveJob(this.ltpPruneJob);

            return base.Disable();
        }

        public override Task Enable()
        {
            this.jobManager.AddJob(this.ltpPruneJob);

            return base.Enable();
        }
    }
}
