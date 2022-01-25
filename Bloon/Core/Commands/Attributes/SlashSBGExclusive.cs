namespace Bloon.Core.Commands.Attributes
{
    using System.Threading.Tasks;
    using Bloon.Variables;
    using DSharpPlus.SlashCommands;

    public class SlashSBGExclusive : SlashCheckBaseAttribute
    {
        public override Task<bool> ExecuteChecksAsync(InteractionContext ctx)
        {
            return Task.FromResult(ctx.Guild?.Id == Guilds.SBG);
        }
    }
}
