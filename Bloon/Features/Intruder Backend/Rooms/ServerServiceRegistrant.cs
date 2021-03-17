namespace Bloon.Features.IntruderBackend.Servers
{
    using Bloon.Core.Services;
    using Microsoft.Extensions.DependencyInjection;

    public class ServerServiceRegistrant : IServiceRegistrant
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<RoomService>();
            services.AddSingleton<ServerJob>();
        }
    }
}
