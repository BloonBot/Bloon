namespace Bloon.Features.Wiki
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.SlashCommands;

    public class WikiFeature : Feature
    {
        private readonly CommandsNextExtension cNext;
        private readonly JobManager jobManager;
        private readonly WikiJob wikiJob;
        private readonly SlashCommandsExtension slash;

        public WikiFeature(DiscordClient dClient, JobManager jobManager, WikiJob wikiJob)
        {
            this.cNext = dClient.GetCommandsNext();
            this.jobManager = jobManager;
            this.wikiJob = wikiJob;
            this.slash = dClient.GetSlashCommands();
        }

        public override string Name => "Fetch Wiki articles";

        public override string Description => "Every X minutes, the bot will fetch any new Wiki articles.";

        public override Task Disable()
        {
            this.cNext.UnregisterCommands<WikiCommands>();
            this.jobManager.RemoveJob(this.wikiJob);

            return base.Disable();
        }

        public override Task Enable()
        {
            this.cNext.RegisterCommands<WikiCommands>();
            this.jobManager.AddJob(this.wikiJob);
            this.slash.RegisterCommands<WikiSlashCommands>(Guilds.SBG);

            return base.Enable();
        }
    }
}
