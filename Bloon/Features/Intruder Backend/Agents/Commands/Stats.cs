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
    using Bloon.Variables;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    [LimitedChannels]
    public class Stats : BaseCommandModule
    {
        private readonly AgentService agentService;

        public Stats(AgentService agentService)
        {
            this.agentService = agentService;
        }

        [Command("stats")]
        [Description("Gets the stats for a particular agent.")]
        [Cooldown(5, 300, CooldownBucketType.User)]
        public async Task AgentStats(CommandContext ctx, [RemainingText] string steamIDOrUsername)
        {
            // Find Agents
            List<Agent> agents = await this.agentService.SearchAgent(steamIDOrUsername);

            // Build Base Embed.
            DiscordEmbedBuilder userDetails = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = $"IntruderFPS API",
                },
                Color = new DiscordColor(95, 95, 95),
            };

            // If the count is 0, no agents found with that name/i
            if (agents.Count == 0)
            {
                userDetails.AddField($"No agent found", "Try a different search");
            }

            // more than one agents returned.
            if (agents.Count > 1)
            {
                string extraAgents = string.Empty;
                int addedAgents = 0;
                foreach (Agent agent in agents)
                {
                    addedAgents++;
                    if (addedAgents > 12)
                    {
                        break;
                    }
                    else
                    {
                        extraAgents += $"{agent.Name} | {agent.SteamID}\n";
                    }
                }

                userDetails.AddField($"Did you mean any of these agents?", extraAgents);
            }

            if (agents.Count == 1)
            {
                Agent agent = agents.FirstOrDefault();

                // See if local DB has agent stored or not.
                if (this.agentService.CheckDBAgent(agent.SteamID))
                {
                    await this.agentService.UpdateDBAgentAsync(agent);
                }
                else
                {
                    // No DB agent found, lets store them.
                    await this.agentService.StoreAgentDBAsync(agent.SteamID);
                }

                AgentStats agentStats = await this.agentService.GetAgentStatsAsync(agent.SteamID);
                userDetails.Url = $"https://intruderfps.com/agents/{agent.SteamID}/";
                float levelProgression = 0;
                float matchWLPercent = 0;
                float roundsWLPercent = 0;
                float killDeathPercent = 0;
                try
                {
                    float totalRounds = agentStats.RoundsWonCapture + agentStats.RoundsWonHack + agentStats.RoundsWonElim + agentStats.RoundsWonTimer + agentStats.RoundsWonCustom + agentStats.RoundsTied;
                    float totalRoundsWon = totalRounds - agentStats.RoundsTied;
                    totalRounds += agentStats.RoundsLost;
                    float totalMatches = agentStats.MatchesWon + agentStats.MatchesLost;
                    matchWLPercent = agentStats.MatchesWon / totalMatches * 100;
                    roundsWLPercent = totalRoundsWon / totalRounds * 100;
                    float totalKills = agentStats.Kills + agentStats.Deaths;
                    killDeathPercent = agentStats.Kills / (float)agentStats.Deaths;
                    float lvlXpRequired = 0;
                    if (agentStats.LevelXPRequired is null)
                    {
                        lvlXpRequired = 0;
                    }
                    else
                    {
                        lvlXpRequired = (float)agentStats.LevelXPRequired;
                    }

                    levelProgression = (agentStats.LevelXP / (float)lvlXpRequired) * 100;
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine($"Found a user but had no match or round data | User: {agent.Name} | {agent.SteamID}");
                }

                string lvlProg = string.Empty;
                double lvlProgress = Math.Floor((double)Math.Round(levelProgression, 0) / 10);

                for (int i = 0; i < (int)lvlProgress; i++)
                {
                    lvlProg += "▰";
                }

                if (float.IsNaN(killDeathPercent))
                {
                    matchWLPercent = 0;
                    roundsWLPercent = 0;
                    killDeathPercent = 0;
                }

                userDetails.Footer.Text = $"Matches W/L: {Math.Round(float.IsNaN(matchWLPercent) ? 0 : matchWLPercent, 1)}% | Rounds W/L: {Math.Round(float.IsNaN(roundsWLPercent) ? 0 : roundsWLPercent, 1)}% | K/D: {(float.IsNaN(killDeathPercent) ? "0" : killDeathPercent.ToString("0.00", CultureInfo.CurrentCulture))} | Time Played: {agentStats.TimePlayed / 3600} Hrs";

                userDetails.Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = agent.AvatarURL,
                };

                userDetails.Title = $"{agent.Name}   //   Level: {agentStats.Level}";

                userDetails.Description =
                    $"{agentStats.LevelXP}*xp* `{lvlProg.PadRight(10, '▱')}` {agentStats.LevelXPRequired}*xp* - {Math.Round(levelProgression, 1)}% \n" +
                    $"**Arrests**: `{agentStats.Arrests}` | **Captures**: `{agentStats.Captures}` | **Hacks**: `{agentStats.NetworkHacks}`\n";
            }

            if (ctx.Channel.IsThread || ctx.Channel.IsPrivate)
            {
                await ctx.RespondAsync(embed: userDetails.Build());
            }
            else
            {
                ThreadQueryResult ctxArchivedThreads = await ctx.Channel.ListPublicArchivedThreadsAsync();
                IReadOnlyList<DiscordThreadChannel> publicArchThreads = ctxArchivedThreads.Threads;

                // Check to see if we have archived threads for this user.
                foreach (DiscordThreadChannel thread in publicArchThreads)
                {
                    if (thread.CreatorId == Users.Bloon)
                    {
                        IReadOnlyList<DiscordThreadChannelMember> threadMembers = await thread.ListJoinedMembersAsync();
                        foreach (DiscordThreadChannelMember user in threadMembers)
                        {
                            // User has an open thread for this command.
                            if (user.Id == ctx.User.Id && thread.Name.Contains($"{ctx.Member.DisplayName} -") && thread.ThreadMetadata.IsArchived)
                            {
                                await thread.SendMessageAsync(embed: userDetails.Build());
                                return;
                            }
                        }
                    }
                }

                // Check if we have any public open threads for this user.
                IReadOnlyList<DiscordThreadChannel> publicThreads = ctx.Channel.Threads;
                foreach (DiscordThreadChannel thread in publicThreads)
                {
                    if (thread.CreatorId == Users.Bloon)
                    {
                        IReadOnlyList<DiscordThreadChannelMember> threadMembers = await thread.ListJoinedMembersAsync();
                        foreach (DiscordThreadChannelMember user in threadMembers)
                        {
                            // User has an open thread for this command.
                            if (user.Id == ctx.User.Id && thread.Name.Contains($"{ctx.Member.DisplayName} -") && !thread.ThreadMetadata.IsArchived)
                            {
                                await thread.SendMessageAsync(embed: userDetails.Build());
                                return;
                            }
                        }
                    }
                }

                var mess = await ctx.Message.CreateThreadAsync($"{ctx.Member.DisplayName} - {steamIDOrUsername} Stats", archiveAfter: DSharpPlus.AutoArchiveDuration.Hour, reason: "User ran .stats command");
                await mess.SendMessageAsync(embed: userDetails.Build());
                return;
            }
        }

        [Command("hiscores")]
        [Aliases("hs", "hiscore", "highscores", "highscore", "top")]
        [Cooldown(5, 300, CooldownBucketType.User)]
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
                    IconUrl = DiscordEmoji.FromGuildEmote(ctx.Client, Variables.Emojis.SBG.Superboss).Url,
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

        [OwnersExclusive]
        [Command("agentstats")]
        [Hidden]
        public async Task PopulateAgentTable(CommandContext ctx, string launchCode)
        {
            await ctx.RespondAsync("OK - starting the table population.");
            if (launchCode == "103967428408512512")
            {
                await this.agentService.PopulateAgentTableAsync();
            }

            await ctx.RespondAsync("Finished table population.");

            // List<Agent> agents = await this.agentService.GetAllAgents(null, null, null, 1, 100);
        }
    }
}
