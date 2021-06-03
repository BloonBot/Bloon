namespace Bloon.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using Bloon.Core.Services;
    using Microsoft.Extensions.Hosting;

    public class CoreService : IHostedService
    {
        private readonly FeatureManager featureManager;
        private readonly JobManager jobManager;

        public CoreService(FeatureManager featureManager, JobManager jobManager)
        {
            this.featureManager = featureManager;
            this.jobManager = jobManager;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.featureManager.InitializeAsync();

            await this.featureManager.Start();

            this.jobManager.Start();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
