namespace Bloon.Features.FAQ
{
    using Bloon.Core.Services;
    using Microsoft.Extensions.DependencyInjection;

    public class FAQServiceRegistrant : IServiceRegistrant
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<FAQManager>();
        }
    }
}
