namespace Bloon.Features.Helprace
{
    using Bloon.Core.Services;
    using Microsoft.Extensions.DependencyInjection;

    public class HelpraceServiceRegistrant : IServiceRegistrant
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<HelpraceService>()
                .ConfigureHttpClient(client =>
                {
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });
            services.AddSingleton<HelpraceJob>();
        }
    }
}
