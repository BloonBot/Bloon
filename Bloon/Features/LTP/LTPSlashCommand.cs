namespace Bloon.Features.LTP
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Variables.Roles;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.SlashCommands;

    public class LTPSlashCommand : ApplicationCommandModule
    {
        [SlashCommand("ltp", "Join or leave the Looking to Play role.")]
        public async Task LTPAsync(InteractionContext ctx)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Success!"));
        }
    }
}
