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

    public class SBAActivityTrackingFeature : Feature
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly DiscordClient dClient;

        public SBAActivityTrackingFeature(IServiceScopeFactory scopeFactory, DiscordClient dClient)
        {
            this.scopeFactory = scopeFactory;
            this.dClient = dClient;
        }

        public override string Name => "SBA Activity Tracking";

        public override string Description => "Tracks message history of users within SBA.";

        public override Task Initialize()
        {
            this.dClient.MessageCreated += this.TrackSBAAsync;

            return base.Initialize();
        }

        public override Task Disable()
        {
            return base.Disable();
        }

        public override Task Enable()
        {
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
