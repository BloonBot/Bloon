namespace Bloon.Features.ModTools
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Bloon.Analytics.Users;
    using Bloon.Features.Analytics;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    [Group("userlookup")]
    [Aliases("ul", "ui")]
    [Description("Report user information to end user.")]
    [ModuleLifespan(ModuleLifespan.Transient)]
    public class UserInfo : BaseCommandModule
    {
        private readonly UserEventService userEventService;

        public UserInfo(UserEventService userEventService)
        {
            this.userEventService = userEventService;
        }

        [Command("-info")]
        [Aliases("-i")]
        [Description("Shows all information of a particular user.")]
#pragma warning disable CA1822 // Mark members as static
        public async Task GuildUserLookup(CommandContext ctx, ulong userID, string guildName = null)
#pragma warning restore CA1822 // Mark members as static
        {
            ulong guildId;

            switch (guildName)
            {
                case "bloon":
                case "bd":
                case "bdev":
                case "bloondev":
                    guildId = Guilds.Bloon;
                    break;
                case "leanto":
                case "lean":
                    guildId = Guilds.Leanto;
                    break;
                case "fox":
                case "foxhound":
                case "fhound":
                case "fh":
                    guildId = Guilds.Leanto;
                    break;
                default:
                    guildId = Guilds.SBG;
                    break;
            }

            DiscordGuild guild = await ctx.Client.GetGuildAsync(guildId);

            DiscordMember member = await guild.GetMemberAsync(userID);

            Permissions memberPerms = member.PermissionsIn(guild.GetDefaultChannel());

            DiscordEmbedBuilder userDetails = new ()
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = $"Status: {member.Presence.Status} | isBot: {(member.IsBot ? "✔" : "✘")}",
                },
                Color = new DiscordColor(95, 95, 95),
                Timestamp = DateTime.UtcNow,
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = member.AvatarUrl,
                },
                Title = "**User Details**",
            };
            userDetails.WithDescription(
                $"**User**: {member.Username}#{member.Discriminator}\n"
                + $"**Nickname**: {member.Nickname}\n"
                + $"**ID**: {member.Id}\n"
                + $"**Avatar Hash**: {member.AvatarHash}\n"
                + $"**Current Activity**: {member.Presence.Activity.Name ?? "-"}\n"
                + $"**IsDeafened**: {member.IsDeafened}\n"
                + $"**IsMuted**: {member.IsMuted}\n"
                + $"**IsSuppressed**: {member.VoiceState?.IsSuppressed}\n"
                + $"**Self Deafened**: {member.VoiceState?.IsSelfDeafened}\n"
                + $"**Self Muted**: {member.VoiceState?.IsSelfMuted}\n"
                + $"**Voice Channel**: {member.VoiceState?.Channel}\n"
                + $"**Voice State**: {member.VoiceState}\n"
                + $"**Guild Permissions**: {member.PermissionsIn(guild.GetDefaultChannel())}\n"
                + $"**Nick**: {member.Nickname}\n"
                + $"**Account Created**: {member.CreationTimestamp.UtcDateTime.ToString("D", CultureInfo.InvariantCulture)}\n"
                + $"**Joined Channel**: {member.JoinedAt.UtcDateTime.ToString("D", CultureInfo.InvariantCulture)}\n");

            string permissions = $"**Add Reactions**: {memberPerms.HasPermission(Permissions.AddReactions)}\n"
                + $"Administrator: {memberPerms.HasPermission(Permissions.Administrator)}\n"
                + $"**Attach Files**: {memberPerms.HasPermission(Permissions.AttachFiles)}\n"
                + $"Ban Members: {memberPerms.HasPermission(Permissions.BanMembers)}\n"
                + $"**Change Nickname**: {memberPerms.HasPermission(Permissions.ChangeNickname)}\n"
                + $"Connect: {memberPerms.HasPermission(Permissions.UseVoice)}\n"
                + $"Create II: {memberPerms.HasPermission(Permissions.CreateInstantInvite)}\n"
                + $"**Embed Links**: {memberPerms.HasPermission(Permissions.EmbedLinks)}\n"
                + $"Kick Members: {memberPerms.HasPermission(Permissions.KickMembers)}\n"
                + $"**Manage Channels**: {memberPerms.HasPermission(Permissions.ManageChannels)}\n"
                + $"Manage Emojis: {memberPerms.HasPermission(Permissions.ManageEmojis)}\n"
                + $"**Manage Guild**: {memberPerms.HasPermission(Permissions.ManageGuild)}\n"
                + $"Manage Messages: {memberPerms.HasPermission(Permissions.ManageMessages)}\n"
                + $"**Manage Nicknames**: {memberPerms.HasPermission(Permissions.ManageNicknames)}\n"
                + $"Manage Roles: {memberPerms.HasPermission(Permissions.ManageRoles)}\n"
                + $"**Manage Webhooks**: {memberPerms.HasPermission(Permissions.ManageWebhooks)}\n"
                + $"Mention Everyone: {memberPerms.HasPermission(Permissions.MentionEveryone)}\n"
                + $"**Move Members**: {memberPerms.HasPermission(Permissions.MoveMembers)}\n"
                + $"Mute Members: {memberPerms.HasPermission(Permissions.MuteMembers)}\n"
                + $"**Read Message History**: {memberPerms.HasPermission(Permissions.ReadMessageHistory)}\n"
                + $"Read Messages: {memberPerms.HasPermission(Permissions.AccessChannels)}\n"
                + $"**Send Messages**: {memberPerms.HasPermission(Permissions.SendMessages)}\n"
                + $"Send TTS Messages: {memberPerms.HasPermission(Permissions.SendTtsMessages)}\n"
                + $"**Speak**: {memberPerms.HasPermission(Permissions.Speak)}\n"
                + $"Use External Emojis: {memberPerms.HasPermission(Permissions.UseExternalEmojis)}\n"
                + $"**Use VAD**: {memberPerms.HasPermission(Permissions.UseVoiceDetection)}\n";

            userDetails.AddField("Guild Permissions", permissions);

            await ctx.RespondAsync(embed: userDetails.Build());
        }

        [Command("-events")]
        [Aliases("-e")]
        [Description("Shows all events of a particular user.")]
        public async Task SearchOnlineAgents(CommandContext ctx, [RemainingText] string discordId)
        {
            List<UserEvent> userEvents = this.userEventService.QueryEventReportAsync(ulong.Parse(discordId, CultureInfo.CurrentCulture));
            DiscordGuild sbg = await ctx.Client.GetGuildAsync(Guilds.SBG);
            DiscordMember member;
            try
            {
                member = await ctx.Guild.GetMemberAsync(ulong.Parse(discordId, CultureInfo.CurrentCulture));
            }
            catch (DSharpPlus.Exceptions.NotFoundException)
            {
                member = await sbg.GetMemberAsync(ulong.Parse(discordId, CultureInfo.CurrentCulture));
            }

            string eventText = string.Empty;
            string timestamps = string.Empty;

            foreach (UserEvent events in userEvents)
            {
                if (events.Event == Event.Joined)
                {
                    eventText = eventText + DiscordEmoji.FromGuildEmote(ctx.Client, Variables.Emojis.EventEmojis.Join) + " Joined\n";
                }

                if (events.Event == Event.Left)
                {
                    eventText = eventText + DiscordEmoji.FromGuildEmote(ctx.Client, Variables.Emojis.EventEmojis.Leave) + " Left\n";
                }

                if (events.Event == Event.Banned)
                {
                    eventText = eventText + DiscordEmoji.FromGuildEmote(ctx.Client, Variables.Emojis.EventEmojis.Banned) + " Banned\n";
                }

                if (events.Event == Event.Unbanned)
                {
                    eventText = eventText + DiscordEmoji.FromGuildEmote(ctx.Client, Variables.Emojis.EventEmojis.Edited) + " Unbanned\n";
                }

                timestamps += $"{events.Timestamp.ToString("ddd, MMM d, yyyy", CultureInfo.CurrentCulture)}\n";
            }

            DiscordEmbedBuilder userDetails = new ()
            {
                Footer = new DiscordEmbedBuilder.EmbedFooter()
                {
                    Text = $"User Report • All Timestamps are EST/US-NY",
                },
                Color = new DiscordColor(95, 95, 95),
                Title = $"**User Report** | {member.Username}",
                Description = $"**User**: {member.Username}#{member.Discriminator}\n" +
                    $"**ID**: {member.Id}\n" +
                    $"**Account Created**: {member.CreationTimestamp.UtcDateTime.ToString("D", CultureInfo.InvariantCulture)}",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = member.AvatarUrl,
                },
            };
            userDetails.AddField($"Events", eventText, inline: true);
            userDetails.AddField($"Timestamps", timestamps, inline: true);

            await ctx.RespondAsync(string.Empty, embed: userDetails.Build());
        }
    }
}
