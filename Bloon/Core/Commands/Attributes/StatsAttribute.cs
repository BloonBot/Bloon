namespace Bloon.Core.Commands.Attributes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Bloon.Variables;
    using Bloon.Variables.Channels;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;

    /**
     * <summary>
     * Precondition for stats commands.
     * </summary>
     */
    public class StatsAttribute : CheckBaseAttribute
    {
        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            if (ctx.Guild?.Id == Guilds.Bloon || ctx.Channel.IsPrivate)
            {
                return Task.FromResult(true);
            }

            if (ctx.Guild?.Id == Guilds.SBG)
            {
                List<ulong> availChannels = new List<ulong>
                {
                    SBGChannels.General,
                    SBGChannels.TradingPost,
                    SBGChannels.CompetitiveMatches,
                    SBGChannels.AUG,
                    SBGChannels.Wiki,
                    SBGChannels.Offtopic,
                };

                return Task.FromResult(availChannels.Contains(ctx.Channel.Id));
            }

            return Task.FromResult(false);
        }
    }
}
