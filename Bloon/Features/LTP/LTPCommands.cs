namespace Bloon.Features.LTP
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Commands.Attributes;
    using Bloon.Core.Database;
    using Bloon.Variables.Roles;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    [ModuleLifespan(ModuleLifespan.Transient)]
    [LimitedChannels(additionalChannels: 892874024403763211)]
    public class LTPCommands : BaseCommandModule
    {
        private readonly BloonContext db;

        public LTPCommands(BloonContext db)
        {
            this.db = db;
        }

        [Command("ltp")]
        [Description("Join or leave the looking to play role.")]
        [Aliases("lookingtoplay", "je", "le")]
        public async Task LTPAsync(CommandContext ctx)
        {
            DiscordRole ltp = ctx.Guild.GetRole(SBGRoles.LookingToPlay);

            // User is already within the looking to play role, remove them.
            if (ctx.Member.Roles.Any(r => r.Id == SBGRoles.LookingToPlay))
            {
                await ctx.Member.RevokeRoleAsync(ltp);
                this.db.Remove(new LTPJoin()
                {
                    UserId = ctx.User.Id,
                });
            }

            // User is not within the looking to play role, promote them to it.
            else
            {
                await ctx.Member.GrantRoleAsync(ltp);
                this.db.LTPJoins.Add(new LTPJoin()
                {
                    UserId = ctx.User.Id,
                    Timestamp = DateTime.UtcNow,
                });
            }

            await this.db.SaveChangesAsync();
            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":heavy_check_mark:"));
        }
    }
}
