namespace Bloon.Features.NowPlaying
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Features.LTP;
    using Bloon.Variables.Roles;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using Microsoft.Extensions.DependencyInjection;

    public class NowPlayingFeature : Feature
    {
        private readonly JobManager jobManager;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly DiscordClient dClient;
        private readonly NowPlayingJob nowPlayingJob;

        public NowPlayingFeature(JobManager jobManager, IServiceScopeFactory scopeFactory, DiscordClient dClient, BloonLog bloonLog)
        {
            this.jobManager = jobManager;
            this.scopeFactory = scopeFactory;
            this.dClient = dClient;
            this.nowPlayingJob = new NowPlayingJob(dClient, bloonLog);
        }

        public override string Name => "Add users to @Now Playing";

        public override string Description => "Automatically adds or removes the @Now Playing role from a member if they are playing Intruder. Also controls the job to check for misfits in NP.";

        public override Task Disable()
        {
            this.jobManager.RemoveJob(this.nowPlayingJob);
            this.dClient.PresenceUpdated -= this.ManageNowPlayingAsync;

            return base.Disable();
        }

        public override Task Enable()
        {
            this.jobManager.AddJob(this.nowPlayingJob);
            this.dClient.PresenceUpdated += this.ManageNowPlayingAsync;

            return base.Enable();
        }

        private Task ManageNowPlayingAsync(DiscordClient dClient, PresenceUpdateEventArgs args)
        {
            Task.Run(async () =>
            {
                // Ignore non-SBG events
                if (args.PresenceAfter.Guild.Id != Variables.Guilds.SBG)
                {
                    return;
                }

                DiscordRole nowPlayingRole = args.PresenceAfter.Guild.GetRole(SBGRoles.NowPlaying);
                DiscordMember member = await args.PresenceAfter.Guild.GetMemberAsync(args.UserAfter.Id);
                bool wasPlaying = (args.PresenceBefore?.Activities.Any(a => a.Name == "Intruder") ?? false) || member.Roles.Any(r => r.Id == SBGRoles.NowPlaying);
                bool nowPlaying = args.PresenceAfter.Activities.Any(a => a.Name == "Intruder");

                // User started playing Intruder
                if (!wasPlaying && nowPlaying)
                {
                    await member.GrantRoleAsync(nowPlayingRole);
                    using IServiceScope scope = this.scopeFactory.CreateScope();
                    using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
                    LTPJoin join = db.LTPJoins.Where(l => l.UserId == args.UserAfter.Id).FirstOrDefault();

                    if (join != null)
                    {
                        join.Timestamp = DateTime.UtcNow;
                        await db.SaveChangesAsync();
                    }
                }

                // User stopped playing Intruder
                else if (wasPlaying && (!nowPlaying || args.PresenceAfter.Status == UserStatus.Invisible || args.PresenceAfter.Status == UserStatus.Offline))
                {
                    await member.RevokeRoleAsync(nowPlayingRole);
                }
            });

            return Task.CompletedTask;
        }
    }
}
