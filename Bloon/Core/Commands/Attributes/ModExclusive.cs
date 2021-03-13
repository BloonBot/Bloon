namespace Bloon.Core.Commands.Attributes
{
    using System.Threading.Tasks;
    using Bloon.Variables;
    using Bloon.Variables.Channels;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;

    /**
     * <summary>
     * Precondition for Mod-Exclusives.
     * </summary>
     */
    public class ModExclusive : CheckBaseAttribute
    {
        // Accept command if user is
        // 1) A mod within SBG
        // 2) Command is ran in #aug, #ground0, or #admins
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help) => Task.FromResult(ctx.User?.Id == Users.DukeofSussex || ctx.User?.Id == Users.Ruby || ctx.User?.Id == Users.RobStorm || ctx.Channel?.Id == BloonChannels.CommandCentre);
    }
}
