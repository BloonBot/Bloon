namespace Bloon.Features.IntruderBackend.Servers
{
    using System.Threading.Tasks;
    using Bloon.Core.Services;

    public class IntruderServersFeature : Feature
    {
        private readonly JobManager jobManager;
        private readonly ServerJob serverJob;

        public IntruderServersFeature(JobManager jobManager, ServerJob serverJob)
        {
            this.jobManager = jobManager;
            this.serverJob = serverJob;
        }

        public override string Name => "Servers";

        public override string Description => "Scrapes the Intruder API for room info and updates the embed located within #current-server-info";

        public override Task Disable()
        {
            this.jobManager.RemoveJob(this.serverJob);

            return base.Disable();
        }

        public override Task Enable()
        {
            this.jobManager.AddJob(this.serverJob);
            return base.Enable();
        }
    }
}
