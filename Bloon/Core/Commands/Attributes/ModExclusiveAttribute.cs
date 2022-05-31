namespace Bloon.Core.Commands.Attributes
{
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Variables.Roles;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;

    /**
     * <summary>
     * Precondition for Mod-Exclusives.
     * </summary>
     */
    public class ModExclusiveAttribute : CheckBaseAttribute
    {
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            return Task.FromResult(ctx.Member?.Roles?.Any(r => r.Id == SBGRoles.Mod) ?? false);
        }
    }
}
