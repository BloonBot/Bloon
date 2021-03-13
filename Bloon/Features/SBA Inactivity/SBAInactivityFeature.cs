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

        public override string Description => "Warns @SBA users after 7 days of inactivity and after 14 days will kick them from the @SBA role.";

        public override Task Disable()
        {
            this.dClient.MessageCreated -= this.TrackSBAAsync;
            this.jobManager.RemoveJob(this.sbaInactivityJob);

            return base.Disable();
        }

        public override Task Enable()
        {
            this.dClient.MessageCreated += this.TrackSBAAsync;
            this.jobManager.AddJob(this.sbaInactivityJob);

            return base.Enable();
        }

        /// <summary>
        /// Track SBA activity for the inactivity job.
        /// </summary>
        /// <param name="args">Message arguments.</param>
        /// <returns>Task.</returns>
        private async Task TrackSBAAsync(DiscordClient dClient, MessageCreateEventArgs args)
        {
            if (args.Author.IsBot || args.Channel.Id != SBGChannels.SecretBaseAlpha)
            {
                return;
            }

            using IServiceScope scope = this.scopeFactory.CreateScope();
            using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            SBAInactivityTracking tracking = db.SBAInactivityTracking
                .Where(s => s.UserId == args.Author.Id)
                .FirstOrDefault();

            if (tracking == null)
            {
                tracking = new SBAInactivityTracking()
                {
                    UserId = args.Author.Id,
                };

                db.Add(tracking);
            }

            tracking.LastMessage = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
    }
}
