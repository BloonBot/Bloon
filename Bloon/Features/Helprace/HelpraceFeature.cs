namespace Bloon.Features.Helprace
{
    using System.Threading.Tasks;
    using Bloon.Core.Services;

    public class HelpraceFeature : Feature
    {
        private readonly JobManager jobManager;
        private readonly HelpraceJob helpraceJob;

        public HelpraceFeature(JobManager jobManager, HelpraceJob helpraceJob)
        {
            this.jobManager = jobManager;
            this.helpraceJob = helpraceJob;
        }

        public override string Name => "Helprace Jobs";

        public override string Description => "Scrapes the Helprace for any new posts and posts them within #bugs-and-suggestions";

        public override Task Disable()
        {
            this.jobManager.RemoveJob(this.helpraceJob);

            return base.Disable();
        }

        public override Task Enable()
        {
            this.jobManager.AddJob(this.helpraceJob);

            return base.Enable();
        }
    }
}
