namespace Bloon.Features.LTP
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Commands.Attributes;
    using Bloon.Core.Database;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.SlashCommands;

    [SlashModuleLifespan(SlashModuleLifespan.Scoped)]
    public class LTPSlashCommand : ApplicationCommandModule
    {
        private readonly BloonContext db;

        public LTPSlashCommand(BloonContext db)
        {
            this.db = db;
        }

        [SlashCommand("ltp", "Add or remove yourself from the Looking To Play role.")]
        [SlashSBGExclusive]
        public async Task LTPAsyncslash(InteractionContext ctx)
        {
            DiscordRole ltp = ctx.Guild.GetRole(Roles.SBG.LookingToPlay);
            DiscordMember guildUser = await ctx.Guild.GetMemberAsync(ctx.User.Id);
            if (guildUser.Roles.Any(roles => roles.Id == Roles.SBG.LookingToPlay))
            {
                await guildUser.RevokeRoleAsync(ltp);

                this.db.LTPJoins.Remove(new LTPJoin()
                {
                    UserId = ctx.User.Id,
                });

                await this.db.SaveChangesAsync();
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You have been removed from the Looking to Play role.").AsEphemeral(true));
            }
            else
            {
                await guildUser.GrantRoleAsync(ltp);

                this.db.LTPJoins.Add(new LTPJoin()
                {
                    UserId = ctx.User.Id,
                    Timestamp = DateTime.UtcNow,
                });

                await this.db.SaveChangesAsync();
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You have been added to the Looking to Play role!").AsEphemeral(true));
            }
        }
    }
}
