namespace Bloon.Features.RedditGuard
{
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using DSharpPlus;
    using DSharpPlus.EventArgs;
    using Microsoft.Extensions.DependencyInjection;
    using Reddit;

    public class RedditGuardFeature : Feature
    {
        private readonly DiscordClient dClient;
        private readonly NewPostEvent newPostEvent;

        public RedditGuardFeature(IServiceScopeFactory scopeFactory, DiscordClient dClient, RedditClient rClient)
        {
            this.dClient = dClient;
            this.newPostEvent = new NewPostEvent(scopeFactory, dClient, rClient);
        }

        public override string Name => "Reddit";

        public override string Description => "When a new post on Reddit is made, the bot will post an embed in the respected channels.";

        public override Task Disable()
        {
            this.newPostEvent.Unregister();
            return base.Disable();
        }

        public override Task Enable()
        {
            if (Bot.Ready)
            {
                this.newPostEvent.Register();
            }
            else
            {
                this.dClient.Ready += this.OnDClientReady;
            }

            return base.Enable();
        }

        private Task OnDClientReady(DiscordClient dClient, ReadyEventArgs args)
        {
            Task.Run(() => this.newPostEvent.Register());
            return Task.CompletedTask;
        }
    }
}
