namespace Bloon.Features.Censor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Commands.Attributes;
    using Bloon.Core.Database;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;
    using Microsoft.EntityFrameworkCore;

    [Group("censor")]
    [ModExclusive]
    [ModuleLifespan(ModuleLifespan.Transient)]
    public class CensorCommands : BaseCommandModule
    {
        private const int CensorsPerEmbed = 15;

        private readonly BloonContext db;
        private readonly Censorer censorer;

        public CensorCommands(BloonContext db, Censorer censorer)
        {
            this.db = db;
            this.censorer = censorer;
        }

        [Command("add")]
        [Description("Adds a censor pattern")]
        public async Task AddAsync(CommandContext ctx, [RemainingText] string pattern)
        {
            Censor censor = new Censor
            {
                Pattern = pattern,
            };

            this.db.Censors.Add(censor);

            await this.db.SaveChangesAsync();

            this.censorer.AddCensor(censor);

            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":white_check_mark:"));
        }

        [Command("remove")]
        [Description("Removes a censor pattern")]
        public async Task RemoveAsync(CommandContext ctx, int id)
        {
            Censor censor = await this.db.Censors
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (censor == null)
            {
                await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":interrobang:"));
                return;
            }

            this.db.Censors.Remove(censor);

            await this.db.SaveChangesAsync();

            this.censorer.RemoveCensor(censor.Id);

            await ctx.Message.CreateReactionAsync(DiscordEmoji.FromName(ctx.Client, ":white_check_mark:"));
        }

        [Command("list")]
        [Description("Retrieves the list of censors")]
        public async Task ListAsync(CommandContext ctx, int page = 1)
        {
            page = page < 1 ? 1 : page;

            int total = await this.db.Censors.CountAsync();
            List<Censor> found = await this.db.Censors
                .Skip((page - 1) * CensorsPerEmbed)
                .Take(CensorsPerEmbed)
                .ToListAsync();

            string description = found.Count == 0
                ? "PEBCAK!"
                : $"```\n{found.ToMarkdownTable()}```";

            DiscordEmbedBuilder embed = new DiscordEmbedBuilder
            {
                Color = new DiscordColor(217, 187, 19),
                Timestamp = DateTime.UtcNow,
                Title = $"Current Censor Patterns (Page {page}/{(int)Math.Ceiling((decimal)total / CensorsPerEmbed)})",
                Description = description,
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = "View pages with \".censor list <page>\"",
                },
            };

            await ctx.RespondAsync(embed);
        }
    }
}
