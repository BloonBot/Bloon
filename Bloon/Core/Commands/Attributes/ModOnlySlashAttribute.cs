namespace Bloon.Core.Commands.Attributes
{
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Variables;
    using Bloon.Variables.Roles;
    using DSharpPlus.SlashCommands;

    public class ModOnlySlashAttribute : ContextMenuCheckBaseAttribute
    {
        public override async Task<bool> ExecuteChecksAsync(ContextMenuContext ctx)
        {
            // SBG
            if (ctx.Guild.Id == 103933666417217536)
            {
                return await Task.FromResult(ctx.Member?.Roles?.Any(r => r.Id == SBGRoles.Mod) ?? false);
            }

            if (ctx.Guild.Id == 196820438398140417)
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }
    }
}
