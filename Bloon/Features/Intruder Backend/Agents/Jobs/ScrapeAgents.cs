namespace Bloon.Features.IntruderBackend.Agents
{
    using System.Threading.Tasks;
    using Bloon.Core.Services;
    using Bloon.Variables.Emojis;

    public class ScrapeAgents : ITimedJob
    {
        private readonly AgentService agentService;

        public ScrapeAgents(AgentService agentService)
        {
            this.agentService = agentService;
        }

        public ulong Emoji => SBGEmojis.Superboss;

        public int Interval => 5;

        public async Task Execute()
        {
            // Scrape historical agent data.
            await this.agentService.ScrapeHistoricalData();
        }
    }
}
