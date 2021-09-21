namespace Bloon.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Commands.Attributes;
    using Bloon.Features.IntruderBackend.Agents;
    using Bloon.Utils;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    public class HiScores : BaseCommandModule
    {
        private readonly AgentService agentService;

        public HiScores(AgentService agentService)
        {
            this.agentService = agentService;
        }

        [Command("hiscores")]
        [Aliases("hs", "hiscore", "highscores", "highscore", "top")]
        [Description("**Available Orderby Columns -**\n" +
            "`matches`, `matches lost`, `rounds`, `rounds lost`, `rounds tied`, `kills`, `deaths`, `arrests`, `team kills`, `captures`, `hacks`, `network hacks`, `survivals`, `suicides`," +
            " `login count`, `pickups`, `votes`, `xp`, `team damage`, `team knockdowns`, `arrested`, `knocked down`, `rounds won capture`, `rounds won hack`, `rounds won elim`, `rounds won timer`, `rounds won custom`," +
            " `positive votes`, `negative votes`")]
        public async Task Hiscores(CommandContext ctx, [RemainingText] string? orderby)
        {
            if (orderby == null)
            {
                orderby = "xp";
            }
            else
            {
                orderby = orderby.ToLowerInvariant();
            }

            DiscordEmbedBuilder hiscoreEmbed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(ctx.Client, Variables.Emojis.SBGEmojis.Superboss).Url,
                    Text = "Superbossgames API | Run .help top for all available columns",
                },
                Color = new DiscordColor(217, 187, 19),
                Timestamp = DateTime.UtcNow,
            };

            List<AgentsDB> agents = this.agentService.GetDBAgentsAsync(orderby);

            string table = string.Empty;

            switch (orderby)
            {
                case "match":
                case "matches":
                case "matches won":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Matches Won";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Matches = a.MatchesWon,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "matches lost":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Matches Lost";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Matches = a.MatchesLost,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "rounds":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Rounds Won";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Rounds = a.RoundsWonCapture + a.RoundsWonCustom + a.RoundsWonHack + a.RoundsWonHack + a.RoundsWonTimer,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "rounds lost":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Rounds Lost";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Rounds = a.RoundsLost,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "rounds tied":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Rounds Tied";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Rounds = a.RoundsTied,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "kills":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered by Kills";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Kills = a.Kills,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "deaths":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Deaths";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Deaths = a.Deaths,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "arrests":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Arrests";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Arrests = a.Arrests,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "team kills":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Team Kills";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Kills = a.TeamKills,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "captures":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Captures";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Captures = a.Captures,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "hacks":
                case "network hacks":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Network Hacks";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Hacks = a.NetworkHacks,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "survivals":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Survivals";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Survivals = a.Survivals,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "suicides":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Suicides";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Suicides = a.Suicides,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "login count":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Login Count";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Logins = a.LoginCount,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "pickups":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Pickups";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Pickups = a.Pickups,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "votes":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Total Votes";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Votes = a.TotalVotes,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "team damage":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Team Damage";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Damage = a.TeamDamage,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "team knockdowns":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Team Knockdowns";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Knockdowns = a.TeamKnockdowns,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "arrested":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Got Arrested";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Arrested = a.GotArrested,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "got knocked down":
                case "knocked down":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Got Knocked Down";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_KnockedDown = a.GotKnockedDown,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "rounds won capture":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Rounds Won Capture";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Rounds = a.RoundsWonCapture,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "rounds won hack":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Rounds Won Hack";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Rounds = a.RoundsWonHack,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "rounds won elim":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Rounds Won Elimination";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Rounds = a.RoundsWonElim,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "rounds won timer":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Rounds Won Timer";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Rounds = a.RoundsWonTimer,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "rounds won custom":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Rounds Won Custom";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Rounds = a.RoundsWonCustom,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "positive votes":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Positive votes";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Votes = a.PositiveVotes,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;

                case "negative votes":
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Negative Votes";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_Votes = a.NegativeVotes,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;
                default:
                    hiscoreEmbed.Title = "Top 10 Agents | Ordered By Default";
                    table = agents.Select(a => new
                    {
                        A_Username = a.Name.Truncate(12),
                        B_Level = a.Level,
                        C_XP = a.TotalXP,
                        D_Playtime = (a.TimePlayed / 3600) + "H",
                    }).ToMarkdownTable();
                    break;
            }

            if (orderby == "help")
            {
                hiscoreEmbed.Title = "Hiscores Help | Descending Only.";

                hiscoreEmbed.Description = $"**Available Criteria**\n" +
                    $"`matches`, `matches lost`, `rounds`, `rounds lost`, `rounds tied`, `kills`, `deaths`, `arrests`, `team kills`, `captures`, `hacks`, `network hacks`, `survivals`, `suicides`," +
                    $" `login count`, `pickups`, `votes`, `xp`, `team damage`, `team knockdowns`, `arrested`, `knocked down`, `rounds won capture`, `rounds won hack`, `rounds won elim`, `rounds won timer`, `rounds won custom`," +
                    $" `positive votes`, `negative votes`";
            }
            else
            {
                hiscoreEmbed.Description = $"```{table}```";
            }

            await ctx.RespondAsync(embed: hiscoreEmbed);
        }
    }
}
