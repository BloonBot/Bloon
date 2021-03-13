namespace Bloon.Core.Commands.Attributes
{
    using System.Threading.Tasks;
    using Bloon.Variables;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;

    /**
     * <summary>
     * Precondition for SBG-Exclusives.
     * </summary>
     */
    public class SBGExclusiveAttribute : CheckBaseAttribute
    {
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help) => Task.FromResult(ctx.Guild?.Id == Guilds.SBG);
    }
}
