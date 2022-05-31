namespace Bloon.Core.Commands.Attributes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Variables;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.SlashCommands;

    /**
     * <summary>
     * Precondition for most Slash commands.
     * </summary>
     */
    public class SlashLimitedChannelsAttribute : SlashCheckBaseAttribute
    {
        public SlashLimitedChannelsAttribute(bool allowDMs = true, bool ignoreMods = true, params ulong[] additionalChannels)
        {
            this.AllowDMs = allowDMs;
            this.Channels.AddRange(additionalChannels);
            this.IgnoreMods = ignoreMods;
        }

        public bool AllowDMs { get; private set; }

        public List<ulong> Channels { get; } = new List<ulong>()
        {
            Variables.Channels.SBG.BloonCommands,
            Variables.Channels.Bloon.Ground0,
            Variables.Channels.Bloon.CommandCentre,
        };

        public bool IgnoreMods { get; private set; }

        public override Task<bool> ExecuteChecksAsync(InteractionContext ctx)
        {
            return Task.FromResult((this.AllowDMs && ctx.Channel.IsPrivate)
                || ((ctx.Guild?.Id == Guilds.SBG || ctx.Guild?.Id == Guilds.Bloon) && this.Channels.Contains(ctx.Channel.Id))
                || (this.IgnoreMods && (ctx.Member?.Roles?.Any(r => r.Id == Roles.SBG.Mod) ?? false)));
        }
    }
}
