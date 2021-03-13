namespace Bloon.Features.Workshop
{
    using Bloon.Core.Services;
    using Microsoft.Extensions.DependencyInjection;

    public class WorkshopServiceRegistrant : IServiceRegistrant
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<WorkshopService>();
            services.AddSingleton<WorkshopJob>();
        }
    }
}
