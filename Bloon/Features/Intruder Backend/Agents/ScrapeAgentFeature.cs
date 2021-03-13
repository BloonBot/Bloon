namespace Bloon.Features.IntruderBackend.Agents
{
    using System.Threading.Tasks;
    using Bloon.Core.Services;

    public class ScrapeAgentFeature : Feature
    {
        private readonly JobManager jobManager;
        private readonly ScrapeAgents scrapeAgents;

        public ScrapeAgentFeature(JobManager jobManager, ScrapeAgents scrapeAgents)
        {
            this.jobManager = jobManager;
            this.scrapeAgents = scrapeAgents;
        }

        public override string Name => "Get Agent Stats";

        public override string Description => "Scrapes the Intruder API for agent stats for package accounts users.";

        public override Task Disable()
        {
            this.jobManager.RemoveJob(this.scrapeAgents);

            return base.Disable();
        }

        public override Task Enable()
        {
            this.jobManager.AddJob(this.scrapeAgents);
            return base.Enable();
        }
    }
}
