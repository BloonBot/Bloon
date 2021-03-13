namespace Bloon.Features.PackageAccounts
{
    using Bloon.Core.Services;
    using Microsoft.Extensions.DependencyInjection;

    public class PackageAccountsServiceRegistrant : IServiceRegistrant
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<AccountService>();
        }
    }
}
