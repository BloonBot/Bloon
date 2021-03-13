namespace Bloon.Features.FAQ
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Core.Services;
    using Bloon.Variables.Channels;
    using DSharpPlus;
    using DSharpPlus.EventArgs;
    using Microsoft.Extensions.DependencyInjection;

    public class FAQFeature : Feature
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly DiscordClient dClient;

        private Dictionary<Regex, string> faqs;

        public FAQFeature(IServiceScopeFactory scopeFactory, DiscordClient dClient)
        {
            this.scopeFactory = scopeFactory;
            this.dClient = dClient;
        }

        public override string Name => "FAQ";

        public override string Description => "Sends a response dependent on a regular expression stored in the database for any frequently asked questions.";

        public override Task Disable()
        {
            this.dClient.MessageCreated -= this.ProcessFAQAsync;

            return base.Disable();
        }

        public override Task Enable()
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            List<Faq> dbFaqs = db.Faqs.ToList();
            this.faqs = dbFaqs.ToDictionary(k => new Regex(k.Regex, RegexOptions.Compiled | RegexOptions.IgnoreCase), v => v.Message);

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

            if (args.Channel.Id == SBGChannels.Help || args.Channel.Id == SBGChannels.General || args.Channel.Id == BloonChannels.Ground0 || args.Channel.Id == SBGChannels.Mapmaker || args.Channel.Id == SBGChannels.Wiki || args.Channel.Id == SBGChannels.MapMakerShowcase || args.Channel.Id == SBGChannels.SecretBaseAlpha)
            {
                foreach (KeyValuePair<Regex, string> faq in this.faqs)
                {
                    if (faq.Key.IsMatch(args.Message.Content))
                    {
                        await args.Channel.SendMessageAsync(faq.Value).ConfigureAwait(false);
                        break; // Make sure we only send one faq response
                    }
                }
            }
        }
    }
}
