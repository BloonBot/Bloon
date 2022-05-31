namespace Bloon.Core.Commands.Attributes
{
    using System.Threading.Tasks;
    using Bloon.Variables;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;

    /**
     * <summary>
     * Precondition for Owners.
     * </summary>
     */
    public class OwnersExclusiveAttribute : CheckBaseAttribute
    {
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            return Task.FromResult(ctx.User?.Id == Users.DukeofSussex || ctx.User?.Id == Users.Ruby || ctx.User?.Id == Users.RobStorm || ctx.Channel?.Id == Channels.Bloon.CommandCentre);
        }
    }
}
