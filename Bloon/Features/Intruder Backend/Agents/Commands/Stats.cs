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

    [Group("stats")]
    [Description("Retrieves a small description of a specified wiki article along with the URL where the information is pulled from.")]
    [StatsAttribute]
    public class Stats : BaseCommandModule
    {
        private readonly AgentService agentService;

        public Stats(AgentService agentService)
        {
            this.agentService = agentService;
        }

        [GroupCommand]
        [Description("Gets the stats for a particular agent.")]
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

            await ctx.RespondAsync(embed: userDetails.Build());
        }

        [OwnersExclusive]
        [Hidden]
        [Command("agentstats")]
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
