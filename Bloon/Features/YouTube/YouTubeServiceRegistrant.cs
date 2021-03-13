namespace Bloon.Features.Youtube
{
    using Bloon.Core.Services;
    using Microsoft.Extensions.DependencyInjection;

    public class YouTubeServiceRegistrant : IServiceRegistrant
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<YouTubeService>();
            services.AddSingleton<YouTubeJob>();
        }
    }
}
