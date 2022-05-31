namespace Bloon.Features.FAQ
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.EventArgs;
    using Microsoft.Extensions.DependencyInjection;

    public class FAQFeature : Feature
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly DiscordClient dClient;
        private readonly CommandsNextExtension cNext;
        private readonly FAQManager faqManager;

        public FAQFeature(IServiceScopeFactory scopeFactory, DiscordClient dClient, FAQManager faqManager)
        {
            this.scopeFactory = scopeFactory;
            this.dClient = dClient;
            this.cNext = dClient.GetCommandsNext();
            this.faqManager = faqManager;
        }

        public override string Name => "FAQ";

        public override string Description => "Sends a response dependent on a regular expression stored in the database for any frequently asked questions.";

        public override Task Disable()
        {
            //this.cNext.UnregisterCommands<FAQCommands>();
            this.dClient.MessageCreated -= this.ProcessFAQAsync;
            this.faqManager.Reset();

            return base.Disable();
        }

        public override Task Enable()
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            List<Faq> dbFaqs = db.Faqs.ToList();
            this.faqManager.Init(dbFaqs);
            //this.cNext.RegisterCommands<FAQCommands>();
            this.dClient.MessageCreated += this.ProcessFAQAsync;

            return base.Enable();
        }

        /// <summary>
        /// Checks the message to match against the regular expressions stored in the DB. If a match is found, it'll show the corresponding message.
        /// </summary>
        /// <param name="args">Message arguments.</param>
        /// <returns>Task.</returns>
        private async Task ProcessFAQAsync(DiscordClient dClient, MessageCreateEventArgs args)
        {
            if (args.Author.IsBot)
            {
                return;
            }

            if ((args.Channel.Id == Channels.SBG.Help || args.Channel.Id == Channels.SBG.General ||args.Channel.Id == Channels.SBG.BloonCommands || args.Channel.Id == Channels.Bloon.Ground0 || args.Channel.Id == Channels.SBG.Mapmaker || args.Channel.Id == Channels.SBG.Wiki || args.Channel.Id == Channels.SBG.MapMakerShowcase || args.Channel.Id == Channels.SBG.SecretBaseAlpha)
                && this.faqManager.TryForAutoResponse(args.Message.Content, out string response))
            {
                await args.Channel.SendMessageAsync(response);
            }
        }
    }
}
