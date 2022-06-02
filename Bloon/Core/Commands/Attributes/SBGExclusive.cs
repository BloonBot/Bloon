namespace Bloon.Core.Commands.Attributes
{
    using System.Threading.Tasks;
    using Bloon.Variables;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;

    public class SBGExclusive : CheckBaseAttribute
    {
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            return Task.FromResult(ctx.Guild?.Id == Guilds.SBG);
        }
    }
}
