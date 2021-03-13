namespace Bloon.Features.Workshop
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;

    public class WorkshopFeature : Feature
    {
        private readonly JobManager jobManager;
        private readonly WorkshopJob workshopJob;
        private readonly CommandsNextExtension cNext;

        public WorkshopFeature(JobManager jobManager, WorkshopJob workshopJob, DiscordClient dClient)
        {
            this.cNext = dClient.GetCommandsNext();
            this.jobManager = jobManager;
            this.workshopJob = workshopJob;
        }

        public override string Name => "Fetch Workshop Items";

        public override string Description => "Every X minutes the bot will check the Steam Workshop for any new Maps available and posts an embed in the respective channels.";

        public override Task Disable()
        {
            this.cNext.UnregisterCommands<WorkshopCommand>();
            this.jobManager.RemoveJob(this.workshopJob);
            return base.Disable();
        }

        public override Task Enable()
        {
            this.cNext.RegisterCommands<WorkshopCommand>();
            this.jobManager.AddJob(this.workshopJob);
            return base.Enable();
        }
    }
}
