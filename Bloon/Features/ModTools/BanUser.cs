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
    public class BanUser : BaseCommandModule
    {
        private readonly AnalyticsContext db;

        public BanUser(AnalyticsContext db)
        {
            this.db = db;
        }

        [Command("banid")]
        [Description("Bans a user from SuperBossGames server.")]
        [ModExclusive]
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public async Task BanUserAsync(CommandContext ctx, ulong discordId, bool deleteMessages, [RemainingText] string? notes)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            try
            {
                DiscordMember discordUser = await ctx.Guild.GetMemberAsync(discordId);
                if (deleteMessages)
                {
                    await discordUser.BanAsync(0, notes);
                }
                else
                {
                    await discordUser.BanAsync(360, notes);
                }

                this.LogModAction(ctx.User.Id, discordId, ModAction.Banned, notes);
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":heavy_check_mark:"));

            }
            catch (DSharpPlus.Exceptions.NotFoundException e)
            {
                // User was not found via ID. Tell mod.
                await ctx.RespondAsync($"Failed to find a guild member with the user ID of `{discordId}`");
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":warning:"));
            }
        }

        [Command("ban")]
        [Description("Bans a user from SuperBossGames server.")]
        [Aliases("banhammer", "hammertime")]
        [ModExclusive]
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public async Task BanUserAsync(CommandContext ctx, DiscordUser user, bool deleteMessages, [RemainingText] string? notes)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            await this.BanUserAsync(ctx, user.Id, deleteMessages, notes);
        }

        [Command("unbanid")]
        [Description("Bans a user from SuperBossGames server via ID.")]
        [ModExclusive]
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public async Task UnbanUserByIDAsync(CommandContext ctx, ulong discordId, [RemainingText] string? notes)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            try
            {
                DiscordMember discordUser = await ctx.Guild.GetMemberAsync(discordId);

                await discordUser.UnbanAsync(notes);

                this.LogModAction(ctx.User.Id, discordId, ModAction.Unbanned, notes);
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":heavy_check_mark:"));

            }
            catch (DSharpPlus.Exceptions.NotFoundException e)
            {
                // User was not found via ID. Tell mod.
                await ctx.RespondAsync($"Failed to find a guild member with the user ID of `{discordId}`");
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":warning:"));
            }
        }

        [Command("unban")]
        [Description("Unbans a user from the SuperbossGames server.")]
        [Aliases("unyeet")]
        [ModExclusive]
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public async Task UnbanUserAsync(CommandContext ctx, DiscordUser user, [RemainingText] string? notes)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        {
            await this.UnbanUserByIDAsync(ctx, user.Id, notes);
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
