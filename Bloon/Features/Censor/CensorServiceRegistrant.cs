namespace Bloon.Features.Censor
{
    using Bloon.Core.Services;
    using Microsoft.Extensions.DependencyInjection;

    public class CensorServiceRegistrant : IServiceRegistrant
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Censorer>();
        }
    }
}
