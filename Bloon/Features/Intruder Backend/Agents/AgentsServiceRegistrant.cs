namespace Bloon.Features.IntruderBackend.Agents
{
    using Bloon.Core.Services;
    using Bloon.Features.Intruder_Backend.Agents;
    using Microsoft.Extensions.DependencyInjection;

    public class AgentsServiceRegistrant : IServiceRegistrant
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<AgentService>();
            services.AddSingleton<ScrapeAgents>();
            services.AddSingleton<IntruderDBAgentService>();
        }
    }
}
