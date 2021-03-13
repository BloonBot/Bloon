namespace Bloon.Features.RedditGuard
{
    using System;
    using Bloon.Core.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Reddit;

    public class RedditGuardServiceRegistrant : IServiceRegistrant
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new RedditClient(Environment.GetEnvironmentVariable("REDDIT_APP_ID"), Environment.GetEnvironmentVariable("REDDIT_REFRESH_TOKEN"), Environment.GetEnvironmentVariable("REDDIT_APP_SECRET")));
        }
    }
}
