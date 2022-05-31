namespace Bloon.Features.ModTools
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
    [ModExclusive]
    public class MuteUser : BaseCommandModule
    {
        private readonly AnalyticsContext db;

        public MuteUser(AnalyticsContext db)
        {
            this.db = db;
        }

        [Command("muteid")]
        [Description("Mute a particular user. Requires Moderator Role. Usage: `.mute {discordId} {notes}`. Notes are optional.")]
        public async Task MuteUserByIDAsync(CommandContext ctx, ulong discordId, [RemainingText] string? notes)
        {
            try
            {
                DiscordRole muted = ctx.Guild.GetRole(SBGRoles.Muted);
                DiscordMember discordUser = await ctx.Guild.GetMemberAsync(discordId);

                // User is already muted.
                if (discordUser.Roles.Any(r => r.Id == SBGRoles.Muted))
                {
                    await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":warning:"));
                    await ctx.RespondAsync("That user is already muted. Another moderator may have muted them before you. Use `.unmute {userID}` to unmute them.");
                }

                // User is not mute, lets go and shut them up.
                else
                {
                    await discordUser.GrantRoleAsync(muted);
                    this.LogModAction(ctx.User.Id, discordId, ModAction.Muted, notes);
                    await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":heavy_check_mark:"));
                }
            }
            catch (DSharpPlus.Exceptions.NotFoundException)
            {
                // User was not found via ID. Tell mod.
                await ctx.RespondAsync($"Failed to find a guild member with the user ID of `{discordId}`");
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":warning:"));
            }
        }

        [Command("mute")]
        [Description("Mute a particular user. Requires Moderator Role. Usage: `.mute @DiscordUserMention {notes}`. Notes are optional.")]
        [Aliases("m", "mu", "silence", "shh")]
        public async Task MuteUserAsync(CommandContext ctx, DiscordUser user, [RemainingText] string? notes)
        {
            await this.MuteUserByIDAsync(ctx, user.Id, notes);
        }

        [Command("unmuteid")]
        [Description("umid")]
        public async Task UnmuteUserByIDAsync(CommandContext ctx, ulong discordId, [RemainingText] string? notes)
        {
            try
            {
                DiscordRole muted = ctx.Guild.GetRole(SBGRoles.Muted);
                DiscordMember discordUser = await ctx.Guild.GetMemberAsync(discordId);

                // User is already muted.
                if (discordUser.Roles.Any(r => r.Id == SBGRoles.Muted))
                {
                    await discordUser.RevokeRoleAsync(muted);
                    await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":heavy_check_mark:"));
                    this.LogModAction(ctx.User.Id, discordId, ModAction.Unmuted, notes);
                }

                // User is not mute, lets go and shut them up.
                else
                {
                    await ctx.RespondAsync("That user isn't currently muted!");
                    await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":warning:"));
                }
            }
            catch (DSharpPlus.Exceptions.NotFoundException)
            {
                // User was not found via ID. Tell mod.
                await ctx.RespondAsync($"Failed to find a guild member with the user ID of `{discordId}`");
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":warning:"));
            }
        }

        [Command("unmute")]
        [Description("Unmute")]
        public async Task UnmuteUserAsync(CommandContext ctx, DiscordUser user, [RemainingText] string? notes)
        {
            await this.UnmuteUserByIDAsync(ctx, user.Id, notes);
        }

        private async void LogModAction(ulong modID, ulong offenderID, ModAction modEvent, string? notes)
        {
            this.db.ModEvents.Add(new ModEvent()
            {
                ModID = modID,
                Event = modEvent,
                OffenderID = offenderID,
                Notes = notes,
                Timestamp = DateTime.Now,
            });
            await this.db.SaveChangesAsync();
        }
    }
}
