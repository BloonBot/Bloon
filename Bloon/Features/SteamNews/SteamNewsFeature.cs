namespace Bloon.Features.SteamNews
{
    using System.Threading.Tasks;
    using Bloon.Core.Services;

    public class SteamNewsFeature : Feature
    {
        private readonly JobManager jobManager;
        private readonly SteamNewsJob newsJob;

        public SteamNewsFeature(JobManager jobManager, SteamNewsJob newsJob)
        {
            this.jobManager = jobManager;
            this.newsJob = newsJob;
        }

        public override string Name => "Fetch Steam News";

        public override string Description => "Every X minutes the bot will check for new Steam announcements.";

        public override Task Disable()
        {
            this.jobManager.RemoveJob(this.newsJob);
            return base.Disable();
        }

        public override Task Enable()
        {
            this.jobManager.AddJob(this.newsJob);
            return base.Enable();
        }
    }
}
