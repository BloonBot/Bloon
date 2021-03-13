namespace Bloon.Features.Analytics
{
    using Bloon.Core.Services;
    using Microsoft.Extensions.DependencyInjection;

    public class UserEventsServiceRegistrant : IServiceRegistrant
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<UserEventService>();
        }
    }
}
