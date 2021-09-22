namespace Bloon.Features.LTP
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Variables.Roles;
    using DSharpPlus;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;
    using DSharpPlus.SlashCommands;
    using Microsoft.EntityFrameworkCore;

    [SlashModuleLifespan(SlashModuleLifespan.Scoped)]
    public class LTPSlashCommand : ApplicationCommandModule
    {
        private readonly BloonContext db;

        public LTPSlashCommand(BloonContext db)
        {
            this.db = db;
        }

        [SlashCommand("test", "TEST IS A TEST")]
        [Cooldown(1,50000,CooldownBucketType.User)]
        public async Task LTPAsyncslash(InteractionContext ctx)
        {
            DiscordRole ltp = ctx.Guild.GetRole(SBGRoles.LookingToPlay);
            DiscordMember guildUser = await ctx.Guild.GetMemberAsync(ctx.User.Id);
            try
            {
                if (guildUser.Roles.Any(roles => roles.Id == SBGRoles.LookingToPlay))
                {
                    await guildUser.RevokeRoleAsync(ltp);

                    this.db.LTPJoins.Remove(new LTPJoin()
                    {
                        UserId = ctx.User.Id,
                    });

                    await this.db.SaveChangesAsync();
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You have been removed from the Looking to Play role."));
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
                    await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You have been added to the Looking to Play role.!"));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
