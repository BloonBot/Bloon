namespace Bloon.Core.Commands.Attributes
{
    using System.Threading.Tasks;
    using Bloon.Variables;
    using DSharpPlus.SlashCommands;

    public class SlashSBGOnly : SlashCheckBaseAttribute
    {
        public override async Task<bool> ExecuteChecksAsync(InteractionContext ctx)
        {
            return await Task.FromResult(ctx.Guild?.Id == Guilds.SBG);
        }
    }
}
