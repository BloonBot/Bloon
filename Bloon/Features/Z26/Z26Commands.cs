namespace Bloon.Features.Z26
{
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Commands.Attributes;
    using Bloon.Core.Database;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.SlashCommands;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// This class pertains to the commands that can be ran practically anywhere. They're typically short and simple commands to run.
    /// </summary>
    [ModuleLifespan(ModuleLifespan.Transient)]
    [Hidden]
    [SlashLimitedChannels]
    public class Z26Commands : ApplicationCommandModule
    {
        private readonly BloonContext db;

        public Z26Commands(BloonContext db)
        {
            this.db = db;
        }

        [SlashCommand("faq", "Retrieves a Z26 faq response")]
        public async Task FaqAsync(CommandContext ctx, [RemainingText] string argument = null)
        {
            if (string.IsNullOrEmpty(argument))
            {
                await ctx.Channel.SendMessageAsync("~~Oi, donkey! Maybe provide an argument the next time you use this damn command?~~ Z26, stop! No need to be so rude to the stupid human.");
                return;
            }

            Z26Faq faq = await this.db.Z26Faqs.Where(x => x.Name == argument).FirstOrDefaultAsync();

            if (faq == null)
            {
                await ctx.RespondAsync($"Couldn't find an entry for \"{argument}\"");
                return;
            }

            await ctx.RespondAsync(faq.Value);
        }
    }
}
