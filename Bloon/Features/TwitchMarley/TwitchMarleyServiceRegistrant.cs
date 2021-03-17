namespace Bloon.Features.TwitchMarley
{
    using System;
    using Bloon.Core.Services;
    using Microsoft.Extensions.DependencyInjection;
    using TwitchLib.Api;
    using TwitchLib.Api.Core;
    using TwitchLib.Client;

    public class TwitchMarleyServiceRegistrant : IServiceRegistrant
    {
        public void ConfigureServices(IServiceCollection services)
        {
            TwitchAPI twitchAPI = new (settings: new ApiSettings()
            {
                ClientId = Environment.GetEnvironmentVariable("TWITCH_CLIENT_ID"),
                Secret = Environment.GetEnvironmentVariable("TWITCH_CLIENT_SECRET"),
            });

            // [x] Generate the access token once with an expiration span of ~60 days
            // [ ] Let TwitchLib handle the token by generating a new one on every request
            twitchAPI.Settings.AccessToken = twitchAPI.V5.Auth.GetAccessToken();

            services.AddSingleton<TwitchClient>()
                .AddSingleton(twitchAPI);
        }
    }
}
