namespace Bloon.Features.Wiki
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;

    public class WikiFeature : Feature
    {
        private readonly CommandsNextExtension cNext;
        private readonly JobManager jobManager;
        private readonly WikiJob wikiJob;

        public WikiFeature(DiscordClient dClient, JobManager jobManager, WikiJob wikiJob)
        {
            this.cNext = dClient.GetCommandsNext();
            this.jobManager = jobManager;
            this.wikiJob = wikiJob;
        }

        public override string Name => "Fetch Wiki articles";

        public override string Description => "Every X minutes, the bot will fetch any new Wiki articles.";

        public override Task Disable()
        {
            this.cNext.UnregisterCommands<WikiCommands>();
            this.jobManager.RemoveJob(this.wikiJob);

            return base.Disable();
        }

        public override Task Enable()
        {
            this.cNext.RegisterCommands<WikiCommands>();
            this.jobManager.AddJob(this.wikiJob);

            return base.Enable();
        }
    }
}
