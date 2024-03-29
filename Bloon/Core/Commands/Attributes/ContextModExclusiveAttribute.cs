namespace Bloon.Core.Commands.Attributes
{
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Variables;
    using Bloon.Variables.Roles;
    using DSharpPlus.SlashCommands;

    public class ContextModExclusiveAttribute : ContextMenuCheckBaseAttribute
    {
        public override async Task<bool> ExecuteChecksAsync(ContextMenuContext ctx)
        {
            if (ctx.Guild?.Id == Guilds.SBG)
            {
                return await Task.FromResult(ctx.Member?.Roles?.Any(r => r.Id == SBGRoles.Mod) ?? false);
            }

            return await Task.FromResult(ctx.Guild?.Id == Guilds.Bloon);
        }
    }
}
