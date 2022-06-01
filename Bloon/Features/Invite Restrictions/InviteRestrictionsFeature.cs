namespace Bloon.Features.Censor
{
    using System;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using Bloon.Variables.Channels;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;

    public class InviteRestrictionsFeature : Feature
    {
        private readonly DiscordClient dClient;
        private readonly Regex inviteRegex;

        public InviteRestrictionsFeature(DiscordClient dClient)
        {
            this.dClient = dClient;
            this.inviteRegex = new Regex(@"(discord\.gg|discord(?:app)?\.com\/invite)\/[a-zA-Z0-9]+", RegexOptions.Compiled);
        }

        public override string Name => "Remove Discord Invites";

        public override string Description => "When a user joins and posts a Discord invite link, the bot will delete the message.";

        public override Task Disable()
        {
            this.dClient.MessageCreated -= this.RestrictInvitesAsync;

            return base.Disable();
        }

        public override Task Enable()
        {
            this.dClient.MessageCreated += this.RestrictInvitesAsync;

            return base.Enable();
        }

        /// <summary>
        /// Removes Discord invites from users that joined within the last 24 hours.
        /// </summary>
        /// <param name="args">Received Discord Message.</param>
        /// <returns>Task.</returns>
        private async Task RestrictInvitesAsync(DiscordClient dClient, MessageCreateEventArgs args)
        {
            if (args.Guild?.Id != Guilds.SBG || args.Author.IsBot)
            {
                return;
            }

            Match match = this.inviteRegex.Match(args.Message.Content);

            if (!match.Success)
            {
                return;
            }

            DiscordMember member = await args.Channel.Guild.GetMemberAsync(args.Author.Id);

            if ((DateTime.UtcNow - member.JoinedAt.UtcDateTime).TotalHours < 24)
            {
                DiscordChannel channel = args.Channel.Guild.GetChannel(SBGChannels.Bloonside);

                await args.Message.DeleteAsync();
                await args.Channel.SendMessageAsync("Invite link removed (Joined <24 hours ago)");
                await channel.SendMessageAsync($"Removed an invite link from `{args.Author.Username}#{args.Author.Discriminator}` to `{match.Value}` in {args.Channel.Mention}.");
            }
        }
    }
}
