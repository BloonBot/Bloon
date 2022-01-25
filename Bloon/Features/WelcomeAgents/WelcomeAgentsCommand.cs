namespace Bloon.Features.WelcomeAgents
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Bloon.Variables.Channels;
    using Bloon.Variables.Emojis;
    using Bloon.Variables.Messages;
    using Bloon.Variables.Roles;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;
    using Serilog;

    [ModuleLifespan(ModuleLifespan.Transient)]
    public class WelcomeAgentsCommand : BaseCommandModule
    {
        private DiscordEmbedBuilder rulesEmbed = new DiscordEmbedBuilder
        {
            Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = $"SuperbossGames Discord - #rules-and-info | Last updated ",
            },
            Color = new DiscordColor(23, 153, 177),
            Timestamp = DateTime.UtcNow,
            Title = $"**Welcome to the Official Intruders Discord server!**",
            Description = $"**1**. Be respectful, no racism or derogatory attitude \n" +
            $"**2**. No NSFW/shocking/pornographic and political nature posts \n" +
            $"**3**. No trolling. \n" +
            $"**4**. No spamming or text abusing. \n" +
            $"**5**. Do not over message/mention staff. \n" +
            $"**6**. No alternate accounts to dodge moderation action. \n" +
            $"**7**. Post in the correct channels. \n" +
            $"**8**. No advertising non-Intruder content. \n" +
            $"**9**. Respect the staff and follow instructions. Mods are doing their best to make a friendly environment. \n" +
            $"**10**. No discussion of moderator actions in public chats. Contact <@182979075256745985> if you feel wrongfully moderated in accordance with the rules listed above.",
        };

        private DiscordEmbedBuilder roleEmbed = new DiscordEmbedBuilder
        {
            Footer = new DiscordEmbedBuilder.EmbedFooter
            {
                Text = $"SuperbossGames Discord - #rules-and-info | Last updated ",
            },
            Color = new DiscordColor(255, 255, 255),
            Timestamp = DateTime.UtcNow,
            Title = $"**Subscribe to Announcements**",
        };

        [Command("welcomeagents")]
        [Hidden]
        public async Task UpdateWelcomeAgents(CommandContext ctx)
        {
            DiscordChannel rulesAndInfo = ctx.Guild.GetChannel(SBGChannels.RulesAndInfo);
            StringBuilder roleLinks = new StringBuilder();

            roleLinks.Append($"<@&{SBGRoles.Developer}> : The developers of Intruder! \n");
            roleLinks.Append($"<@&{SBGRoles.Nerds}> : Official staff partners working along with the developer for community management.\n");
            roleLinks.Append($"<@&{SBGRoles.Mod}> : The amazing community volunteers assisting the team to keep the peace.  \n");
            roleLinks.Append($"<@&{SBGRoles.AUG}> : A group of serious players who engage the community. \n");
            roleLinks.Append($"<@&{SBGRoles.Agent}> : All members of the community.  \n");

            StringBuilder importLinks = new StringBuilder();
            importLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, PlatformEmojis.YouTube)} | [**Youtube**](https://www.youtube.com/superbossgames)\n");
            importLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, PlatformEmojis.Twitter)} | [**Twitter**](https://twitter.com/SuperbossGames/)\n");
            importLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, PlatformEmojis.Helprace)} | [**Helprace**](https://superbossgames.helprace.com/)\n");
            importLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, PlatformEmojis.Reddit)} | [**Reddit**](https://www.reddit.com/r/Intruder)\n");
            importLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, PlatformEmojis.Twitch)} | [**Twitch**](https://www.twitch.tv/superbossgames)\n");
            importLinks.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, PlatformEmojis.Discord)} | [**Server Invite**](https://discord.gg/superbossgames)\n");

            this.rulesEmbed.AddField($"Roles", roleLinks.ToString(), false);
            this.rulesEmbed.AddField($"Important Links", importLinks.ToString(), false);

            try
            {
                foreach (DiscordMessage msg in await rulesAndInfo.GetMessagesAsync())
                {
                    if (msg.Author.Id == ctx.Client.CurrentUser.Id && msg.Id == 892811059897991208)
                    {
                        await msg.ModifyAsync(embed: this.rulesEmbed.Build());
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e.InnerException, "Failed to edit or update the Current Server Info.");
            }
        }

        [Command("roleembed")]
        [Hidden]
        public async Task UpdateRoleEmbed(CommandContext ctx)
        {
            this.roleEmbed.Description = $"React to this message using {DiscordEmoji.FromGuildEmote(ctx.Client, ManageRoleEmojis.BloonMoji)} reaction to subscribe to news and stay up to date. This way, we will reserve using the @everyone for essential news only.\n";
            DiscordChannel rulesAndInfo = ctx.Guild.GetChannel(SBGChannels.RulesAndInfo);

            StringBuilder roleLinks = new StringBuilder();

            try
            {
                foreach (DiscordMessage msg in await rulesAndInfo.GetMessagesAsync())
                {
                    if (msg.Author.Id == ctx.Client.CurrentUser.Id && msg.Id == SBGMessages.TheOnlyMessageIDWeCurrentlyCareAboutAtleastInAPublicFacingPerspective)
                    {
                        await msg.ModifyAsync(embed: this.rulesEmbed.Build());
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e.InnerException, "Failed to edit or update the Current Server Info.");
            }
        }

    }
}
