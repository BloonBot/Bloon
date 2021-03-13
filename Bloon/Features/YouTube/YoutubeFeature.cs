namespace Bloon.Features.Youtube
{
    using System.Threading.Tasks;
    using Bloon.Core.Services;

    public class YouTubeFeature : Feature
    {
        private readonly JobManager jobManager;
        private readonly YouTubeJob youtubeJob;

        public YouTubeFeature(JobManager jobManager, YouTubeJob youTubeJob)
        {
            this.jobManager = jobManager;
            this.youtubeJob = youTubeJob;
        }

        public override string Name => "Fetch Youtube Videos";

        public override string Description => "Every X minutes, the bot will check the SuperbossGames channel on Youtube for any new Videos.";

        public override Task Disable()
        {
            this.jobManager.RemoveJob(this.youtubeJob);

            return base.Disable();
        }

        public override Task Enable()
        {
            this.jobManager.AddJob(this.youtubeJob);

            return base.Enable();
        }
    }
}
