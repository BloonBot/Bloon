namespace Bloon.Features.ModTools
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Threading.Tasks;
    using Bloon.Analytics.Users;
    using Bloon.Core.Commands.Attributes;
    using Bloon.Features.Analytics;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.SlashCommands;

    [SlashModuleLifespan(SlashModuleLifespan.Scoped)]
    public class UserInfoMenu : ApplicationCommandModule
    {
        private readonly UserEventService userEventService;

        public UserInfoMenu(UserEventService userEventService)
        {
            this.userEventService = userEventService;
        }

        // Runs when a user selects the app via a Discord Message
        [ContextMenu(ApplicationCommandType.MessageContextMenu, "User Channel Perms")]
        [ContextModExclusive]
        public async Task UserInfoFromMessageCommand(ContextMenuContext ctx)
        {
            DiscordGuild guild = await ctx.Client.GetGuildAsync(Guilds.SBG);
            DiscordMember member = await guild.GetMemberAsync(ctx.TargetMessage.Author.Id);
            Permissions memberPerms = member.PermissionsIn(guild.GetChannel(ctx.Channel.Id));

            DiscordEmbedBuilder userDetails = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = $"ID: {member.Id} | isBot: {(member.IsBot ? "✔" : "✘")}",
                },
                Color = new DiscordColor(95, 95, 95),
                Timestamp = DateTime.UtcNow,
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = member.AvatarUrl,
                },
                Title = $"**User Details for {ctx.Channel.Name}**",
            };
            userDetails.WithDescription(this.BuildUserDetails(member));

            userDetails.AddField("Guild Permissions", this.BuildPermissions(memberPerms, ctx));

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(userDetails.Build()).AsEphemeral(true));
        }

        // Runs when a user selects the app via a User
        [ContextMenu(ApplicationCommandType.UserContextMenu, "Get User Info")]
        [ContextModExclusive]
        public async Task UserInfoFromUserMenu(ContextMenuContext ctx)
        {
            DiscordGuild guild = await ctx.Client.GetGuildAsync(Guilds.SBG);
            DiscordMember member = await guild.GetMemberAsync(ctx.TargetMember.Id);

            DiscordEmbedBuilder userDetails = new DiscordEmbedBuilder
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = $"ID: {member.Id} | isBot: {(member.IsBot ? "✔" : "✘")}",
                },
                Color = new DiscordColor(95, 95, 95),
                Timestamp = DateTime.UtcNow,
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = member.AvatarUrl,
                },
                Title = "**User Details**",
            };
            userDetails.WithDescription(this.BuildUserDetails(member));
            string userEvents = this.BuildUserEventsAsync(ctx.TargetMember.Id, ctx);
            if (!string.IsNullOrEmpty(userEvents))
            {
                userDetails.AddField("User Server Events", userEvents);
            }

            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(userDetails.Build()).AsEphemeral(true));
        }

        private string BuildPermissions(Permissions permissions, ContextMenuContext ctx)
        {
            string perms = $"**Add Reactions**: {(permissions.HasPermission(Permissions.AddReactions) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                            + $"Administrator: {(permissions.HasPermission(Permissions.Administrator) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"**Attach Files**: {(permissions.HasPermission(Permissions.AttachFiles) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"Ban Members: {(permissions.HasPermission(Permissions.BanMembers) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"**Change Nickname**: {(permissions.HasPermission(Permissions.ChangeNickname) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"Connect: {(permissions.HasPermission(Permissions.UseVoice) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"Create II: {(permissions.HasPermission(Permissions.CreateInstantInvite) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"**Embed Links**: {(permissions.HasPermission(Permissions.EmbedLinks) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"Kick Members: {(permissions.HasPermission(Permissions.KickMembers) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"**Manage Channels**: {(permissions.HasPermission(Permissions.ManageChannels) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"Manage Emojis: {(permissions.HasPermission(Permissions.ManageEmojis) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"**Manage Guild**: {(permissions.HasPermission(Permissions.ManageGuild) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"Manage Messages: {(permissions.HasPermission(Permissions.ManageMessages) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"**Manage Nicknames**: {(permissions.HasPermission(Permissions.ManageNicknames) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"Manage Roles: {(permissions.HasPermission(Permissions.ManageRoles) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"**Manage Webhooks**: {(permissions.HasPermission(Permissions.ManageWebhooks) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"Mention Everyone: {(permissions.HasPermission(Permissions.MentionEveryone) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"**Move Members**: {(permissions.HasPermission(Permissions.MoveMembers) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"Mute Members: {(permissions.HasPermission(Permissions.MuteMembers) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"**Read Message History**: {(permissions.HasPermission(Permissions.ReadMessageHistory) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"Read Messages: {(permissions.HasPermission(Permissions.AccessChannels) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"**Send Messages**: {(permissions.HasPermission(Permissions.SendMessages) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"Send TTS Messages: {(permissions.HasPermission(Permissions.SendTtsMessages) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"**Speak**: {(permissions.HasPermission(Permissions.Speak) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"Use External Emojis: {(permissions.HasPermission(Permissions.UseExternalEmojis) ? DiscordEmoji.FromName(ctx.Client, ":white_check_mark:") : DiscordEmoji.FromName(ctx.Client, ":x:"))}\n"
                             + $"**Use VAD**: {(permissions.HasPermission(Permissions.UseVoiceDetection) ? "✔" : "✘")}\n";
            return perms;
        }

        private string BuildUserDetails(DiscordMember member)
        {
            string details = $"**User**: {member.Username}#{member.Discriminator}\n"
                + $"**Mention**: <@{member.Id}>\n"
                + $"**Nickname**: {member.Nickname}\n"
                + $"**Avatar URL**: [Click Here]({member.AvatarUrl})\n"
                + $"**IsDeafened**: {member.IsDeafened}\n"
                + $"**IsMuted**: {member.IsMuted}\n"
                + $"**IsSuppressed**: {member.VoiceState?.IsSuppressed}\n"
                + $"**Self Deafened**: {member.VoiceState?.IsSelfDeafened}\n"
                + $"**Self Muted**: {member.VoiceState?.IsSelfMuted}\n"
                + $"**Voice Channel**: {member.VoiceState?.Channel}\n"
                + $"**Voice State**: {member.VoiceState}\n"
                + $"**Nick**: {member.Nickname}\n"
                + $"**Account Created**: {member.CreationTimestamp.UtcDateTime.ToString("D", CultureInfo.InvariantCulture)}\n"
                + $"**Joined Channel**: {member.JoinedAt.UtcDateTime.ToString("D", CultureInfo.InvariantCulture)}\n";

            return details;
        }

        private string BuildUserEventsAsync(ulong discordId, ContextMenuContext ctx)
        {
            List<UserEvent> userEvents = this.userEventService.QueryEventReportAsync(discordId);

            StringBuilder discordUserEvents = new StringBuilder();

            foreach (UserEvent events in userEvents)
            {
                if (events.Event == Event.Joined)
                {
                    discordUserEvents.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Event.Join)} Joined - {events.Timestamp.ToString("ddd, MMM d, yyyy", CultureInfo.CurrentCulture)}\n");
                }

                if (events.Event == Event.Left)
                {
                    discordUserEvents.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Event.Leave)} Left - {events.Timestamp.ToString("ddd, MMM d, yyyy", CultureInfo.CurrentCulture)}\n");
                }

                if (events.Event == Event.Banned)
                {
                    discordUserEvents.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Event.Banned)} Banned - {events.Timestamp.ToString("ddd, MMM d, yyyy", CultureInfo.CurrentCulture)}\n");
                }

                if (events.Event == Event.Unbanned)
                {
                    discordUserEvents.Append($"{DiscordEmoji.FromGuildEmote(ctx.Client, Emojis.Event.Edited)} Unbanned - {events.Timestamp.ToString("ddd, MMM d, yyyy", CultureInfo.CurrentCulture)}\n");
                }
            }

            return discordUserEvents.ToString();
        }
    }
}
