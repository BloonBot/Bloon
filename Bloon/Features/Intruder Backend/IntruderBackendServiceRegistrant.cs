namespace Bloon.Features.IntruderBackend
{
    using Bloon.Core.Services;
    using IntruderLib;
    using Microsoft.Extensions.DependencyInjection;

    public class IntruderBackendServiceRegistrant : IServiceRegistrant
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IntruderAPI>();
        }
    }
}
