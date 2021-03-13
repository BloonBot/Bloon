namespace Bloon.Features.ModTools
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Bloon.Features.IntruderBackend.Agents;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    [Group("agentsearch")]
    [Aliases("as")]
    [Description("Search for a particular Agent")]
    [ModuleLifespan(ModuleLifespan.Transient)]
    public class AgentSearch : BaseCommandModule
    {
        private readonly AgentService agentService;

        public AgentSearch(AgentService agentService)
        {
            this.agentService = agentService;
        }

        /// <summary>
        /// This needs some more work, it'll probably break past 25 agents as we'll hit the character limit of an embed.
        /// Need to limit the amount that shows on the first page. Annoying to test when there is less than 25 people in the first place.
        /// </summary>
        [Command("-online")]
        [Aliases("-o")]
        [Description("Show All Online Users")]
        public async Task SearchOnlineAgents(CommandContext ctx)
        {
            List<Agent> agents = await this.agentService.GetAgents(null, null, true, 1, 25).ConfigureAwait(false);

            DiscordEmbedBuilder onlineUsersEmbed = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    IconUrl = DiscordEmoji.FromGuildEmote(ctx.Client, Variables.Emojis.SBGEmojis.Superboss).Url,
                    Text = $"IntruderFPS API | Total Online Users: {agents.Count}",
                },
                Color = new DiscordColor(95, 95, 95),
                Timestamp = DateTime.UtcNow,
                Title = $"**Current Online Users**",
            };

            string agentDetails = string.Empty;

            foreach (Agent agent in agents)
            {
                agentDetails += $"{agent.IntruderID} | {agent.Name} | `{agent.SteamID}`\n";
            }

            if (agents.Count > 25)
            {
                onlineUsersEmbed.Description = $"It looks like there are more than 25 results. Try narrowing your results.\n" +
                    "Try: .agentsearch -online (int){page} (bool){ShowRooms}\n" +
                    "Example: `.as -o 1 true`";
            }

            onlineUsersEmbed.AddField($"ID  |  NAME | Steam ID ", agentDetails, false);

            await ctx.Channel.SendMessageAsync(string.Empty, embed: onlineUsersEmbed.Build()).ConfigureAwait(false);
        }
    }
}
