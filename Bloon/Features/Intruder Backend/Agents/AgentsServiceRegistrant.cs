namespace Bloon.Features.IntruderBackend.Agents
{
    using Bloon.Core.Services;
    using Microsoft.Extensions.DependencyInjection;

    public class AgentsServiceRegistrant : IServiceRegistrant
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<AgentService>();
        }
    }
}
