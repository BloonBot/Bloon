namespace Bloon.Features.WelcomeAgents
{
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Discord;
    using Bloon.Core.Services;
    using Bloon.Variables.Emojis;
    using Bloon.Variables.Messages;
    using Bloon.Variables.Roles;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;

    public class WelcomeAgentsFeature : Feature
    {
        private readonly DiscordClient dClient;
        private readonly CommandsNextExtension cNext;

        public WelcomeAgentsFeature(DiscordClient dClient)
        {
            this.dClient = dClient;
            this.cNext = dClient.GetCommandsNext();
        }

        public override string Name => "Rules & Info Embeds";

        public override string Description => "Update or post the embeds in #rules-and-info along with role-reaction assignments.";

        public override Task Disable()
        {
            this.cNext.UnregisterCommands<WelcomeAgentsCommand>();

            this.dClient.MessageReactionAdded -= this.NewsRoleAssignment;
            this.dClient.MessageReactionRemoved -= this.DClient_MessageReactionRemoved;

            return base.Disable();
        }

        public override Task Enable()
        {
            this.cNext.RegisterCommands<WelcomeAgentsCommand>();

            this.dClient.MessageReactionAdded += this.NewsRoleAssignment;
            this.dClient.MessageReactionRemoved += this.DClient_MessageReactionRemoved;

            return base.Enable();
        }

        private async Task NewsRoleAssignment(DiscordClient dClient, MessageReactionAddEventArgs args)
        {
            if (args.User.Id == dClient.CurrentUser.Id || (args.Message?.Id != SBGMessages.TheOnlyMessageIDWeCurrentlyCareAboutAtleastInAPublicFacingPerspective && !args.Emoji.Equals(DiscordEmoji.FromGuildEmote(this.dClient, ManageRoleEmojis.BloonMoji))))
            {
                return;
            }

            DiscordMember discordUser = await args.Guild.GetMemberAsync(args.User.Id);
            DiscordRole newsRole = args.Guild.GetRole(SBGRoles.News);

            if (args.Message.Id == SBGMessages.TheOnlyMessageIDWeCurrentlyCareAboutAtleastInAPublicFacingPerspective && !discordUser.Roles.Contains(newsRole))
            {
                await discordUser.GrantRoleAsync(newsRole);
            }
            return;
        }

        private async Task DClient_MessageReactionRemoved(DiscordClient sender, MessageReactionRemoveEventArgs args)
        {
            if (args.User.Id == this.dClient.CurrentUser.Id || (args.Message?.Id != SBGMessages.TheOnlyMessageIDWeCurrentlyCareAboutAtleastInAPublicFacingPerspective && !args.Emoji.Equals(DiscordEmoji.FromGuildEmote(this.dClient, ManageRoleEmojis.BloonMoji))))
            {
                return;
            }

            DiscordMember discordUser = await args.Guild.GetMemberAsync(args.User.Id);
            DiscordRole newsRole = args.Guild.GetRole(SBGRoles.News);

            if (args.Message.Id == SBGMessages.TheOnlyMessageIDWeCurrentlyCareAboutAtleastInAPublicFacingPerspective && discordUser.Roles.Contains(newsRole))
            {
                await discordUser.RevokeRoleAsync(newsRole);
            }

            return;
        }
    }
}
