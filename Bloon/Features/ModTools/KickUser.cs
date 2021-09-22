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
    public class KickUser : BaseCommandModule
    {
        private readonly AnalyticsContext db;

        public KickUser(AnalyticsContext db)
        {
            this.db = db;
        }

        [Command("kickid")]
        [Description("Kicks a user from the guild.")]
        [ModExclusive]
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public async Task KickUserAsync(CommandContext ctx, ulong discordId, [RemainingText] string? notes)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {

            try
            {
                DiscordMember discordUser = await ctx.Guild.GetMemberAsync(discordId);

                await discordUser.RemoveAsync(notes);
                this.LogModAction(ctx.User.Id, discordId, ModActions.Kicked, notes);
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":heavy_check_mark:"));
            }
            catch (DSharpPlus.Exceptions.NotFoundException)
            {
                // User was not found via ID. Tell mod.
                await ctx.RespondAsync($"Failed to find a guild member with the user ID of `{discordId}`");
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":warning:"));
            }
        }

        [Command("kick")]
        [Description("Kicks a user from the guild.")]
        [Aliases("boot", "yeet")]
        [ModExclusive]
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public async Task KickUserAsync(CommandContext ctx, DiscordUser user, [RemainingText] string? notes)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            await this.KickUserAsync(ctx, user.Id, notes);
        }

        private async void LogModAction(ulong modID, ulong offenderID, ModActions modEvent, string? notes)
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
