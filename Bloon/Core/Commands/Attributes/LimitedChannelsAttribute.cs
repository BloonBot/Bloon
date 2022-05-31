namespace Bloon.Core.Commands.Attributes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Variables;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;

    /**
     * <summary>
     * Precondition for most commands.
     * </summary>
     */
    public class LimitedChannelsAttribute : CheckBaseAttribute
    {
        public LimitedChannelsAttribute(bool allowDMs = true, bool ignoreMods = true, params ulong[] additionalChannels)
        {
            this.Channels.AddRange(additionalChannels);
            this.AllowDMs = allowDMs;
            this.IgnoreMods = ignoreMods;
        }

        public List<ulong> Channels { get; } = new List<ulong>()
        {
            Variables.Channels.SBG.BloonCommands,
            Variables.Channels.Bloon.CommandCentre,
            Variables.Channels.Bloon.Commands,
            Variables.Channels.Bloon.Ground0,
        };

        public bool AllowDMs { get; private set; }

        public bool IgnoreMods { get; private set; }

        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            return Task.FromResult(((ctx.Guild?.Id == Guilds.SBG || ctx.Guild?.Id == Guilds.Bloon) && this.Channels.Contains(ctx.Channel.Id))
                || (this.AllowDMs && ctx.Channel.IsPrivate)
                || (this.IgnoreMods && (ctx.Member?.Roles?.Any(r => r.Id == Roles.SBG.Mod) ?? false)));
        }
    }
}
