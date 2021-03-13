namespace Bloon.Features.Wiki
{
    using Bloon.Core.Services;
    using Microsoft.Extensions.DependencyInjection;

    public class WikiServiceRegistrant : IServiceRegistrant
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<WikiService>();
            services.AddSingleton<WikiJob>();
        }
    }
}
