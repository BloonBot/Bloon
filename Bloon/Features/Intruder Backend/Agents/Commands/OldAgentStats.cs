using Bloon.Core.Commands.Attributes;
using Bloon.Features.IntruderBackend.Agents;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bloon.Features.Intruder_Backend.Agents.Commands
{
    [Group("oldstats")]
    [GeneralAttribute]
    public class OldAgentStats : BaseCommandModule
    {
        private readonly IntruderDBAgentService agentService;

        public OldAgentStats(IntruderDBAgentService agentService)
        {
            this.agentService = agentService;
        }

        [GroupCommand]
        [Description("Returns pre-steam Intruder agent data.")]
        public async Task AgentStats(CommandContext ctx, [RemainingText] string steamIDOrUsername)
        {
            IntruderDBAgent agent = await this.agentService.GetDBAgentAsync(steamIDOrUsername);

            // Build Base Embed.
            DiscordEmbedBuilder userDetails = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = $"IntruderDB RIP",
                },
                Color = new DiscordColor(95, 95, 95),
            };

            // If the count is 0, no agents found with that name/i
            if (agent == null)
            {
                userDetails.AddField($"No agent found", "Try a different search");
            }

            userDetails.Title = $"__{agent.Name}__   ///   XP: {agent.XP}";

            userDetails.AddField($"Matches:",
                $"**Won**: `{agent.MatchesWon}` | **Lost**: `{agent.MatchesLost}`\n" +
                $"**Survived**: `{agent.MatchesSurvived}` | **Total**: {agent.MatchesPlayed}\n");

            userDetails.AddField($"Objectives:",
               $"**Arrests**: `{agent.Arrests}` | **Captures**: `{agent.Captures}`\n");

            userDetails.AddField($"Timestamps:",
                $"Last Update: {agent.LastUpdate}\n" +
                $"Last Seen: {agent.LastSeen}\n" +
                $"Registerd: {agent.Registered}", true);

            userDetails.Footer.Text = $"IntruderDB RIP - Time Played: {agent.TimePlayed / 3600} Hrs";
            await ctx.RespondAsync(embed: userDetails.Build());
        }
    }
}
