namespace Bloon.Features.SteamNews
{
    using Bloon.Core.Services;
    using Microsoft.Extensions.DependencyInjection;

    public class SteamNewsServiceRegistrant : IServiceRegistrant
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<SteamNewsService>();
            services.AddSingleton<SteamNewsJob>();
        }
    }
}
