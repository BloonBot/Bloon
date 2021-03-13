namespace Bloon.Features.Twitter
{
    using System;
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using DSharpPlus;
    using Microsoft.Extensions.DependencyInjection;
    using Reddit;

    public class TwitterFeature : Feature, IDisposable
    {
        private readonly JobManager jobManager;
        private readonly TwitterJob twitterJob;

        public TwitterFeature(IServiceScopeFactory scopeFactory, JobManager jobManager, DiscordClient dClient, RedditClient rClient, BloonLog bloonLog)
        {
            this.jobManager = jobManager;
            this.twitterJob = new TwitterJob(scopeFactory, dClient, rClient, bloonLog);
        }

        public override string Name => "Twitter";

        public override string Description => "Like and favorites that shit.";

        public override Task Disable()
        {
            this.jobManager.RemoveJob(this.twitterJob);

            return base.Disable();
        }

        public override Task Enable()
        {
            this.jobManager.AddJob(this.twitterJob);

            return base.Enable();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.twitterJob.Dispose();
            }
        }
    }
}
