namespace Bloon.Features.PackageAccounts
{
    using System;
    using System.Threading.Tasks;
    using Bloon.Variables.Emojis;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    [Group("package")]
    [Description("Commands that manage a user's Package Account")]
    public class AccountsCommands : BaseCommandModule
    {
        private readonly AccountService accountService;

        public AccountsCommands(AccountService accounts)
        {
            this.accountService = accounts;
        }

        [GroupCommand]
        [Description("Running this command by itself will return package related help embed for more in-depth help.")]
        public async Task PackagAccountsGeneral(CommandContext ctx)
        {
            if (this.CheckBasics(ctx) == true)
            {
                DiscordEmbedBuilder accountEmbed = new DiscordEmbedBuilder
                {
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = DiscordEmoji.FromGuildEmote(ctx.Client, ManageRoleEmojis.Warning).Url,
                        Text = "Package Account Help Embed",
                    },
                    Color = new DiscordColor(52, 114, 53),
                    Timestamp = DateTime.UtcNow,
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                    {
                        Url = ctx.User.AvatarUrl,
                    },
                    Title = "Package Account Help",
                };
                accountEmbed.AddField($"**Creating an Account**", $"To create an account, simply run `.package -c`. You can also use `.package -create`. This is the first step to creating an account on Bloon", true);
                accountEmbed.AddField($"**Seeing my Profile**", $"You can see you profile on the website or via Discord in a DM using `.package -p` - This feature is not complete yet.", false);
                accountEmbed.AddField($"**Edit my Profile**", $"You can edit your profile *or delete* via the website or by using `.package -pe`. - This feature is not complete yet.", false);
                accountEmbed.AddField($"**FAQ**", $"To see FAQ use `.package -faq`. Your question might be answered there. If not, try one of the bot admins.", false);

                await ctx.Channel.SendMessageAsync(string.Empty, embed: accountEmbed.Build()).ConfigureAwait(false);
            }
            else
            {
                await ctx.Channel.SendMessageAsync($"This command may only be ran within a DM.").ConfigureAwait(false);
            }
        }

        [Command("-help")]
        [Aliases("-h")]
        [Description("Help information for package accounts.")]
        public async Task PackagAccountsHelp(CommandContext ctx)
        {
            if (this.CheckBasics(ctx) == true)
            {
                await this.PackagAccountsGeneral(ctx).ConfigureAwait(false);
                return;
            }
            else
            {
                await ctx.Channel.SendMessageAsync($"This command may only be ran within a DM.").ConfigureAwait(false);
            }
        }

        [Command("-profile")]
        [Aliases("-p")]
        [Description("Help information for package accounts.")]
        public async Task PackagAccountsProfile(CommandContext ctx)
        {
            if (this.CheckBasics(ctx) == true)
            {
                PackageAccount account = this.accountService.FindAccount(ctx.User.Id);
                DiscordEmbedBuilder accountEmbed = new DiscordEmbedBuilder
                {
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = DiscordEmoji.FromGuildEmote(ctx.Client, CommandEmojis.Run).Url,
                        Text = $"Package Account Profile | Account Created: {account.AccountCreated}",
                    },
                    Color = new DiscordColor(52, 114, 53),
                    Timestamp = DateTime.UtcNow,
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                    {
                        Url = ctx.User.AvatarUrl,
                    },
                };
                if (account.SteamID != null)
                {
                    accountEmbed.AddField($"**Discord ID**", $"`{account.DiscordID}`", false);
                    accountEmbed.AddField($"**Steam ID**", $"`{account.SteamID}`", false);
                    accountEmbed.AddField($"**Private Profile**", $"`{account.PrivateProfile}`", false);
                    accountEmbed.AddField($"**Account Permission**", $"`{account.Type}`", false);
                }

                if (account.SteamID == null)
                {
                    accountEmbed.AddField($"**Discord ID**", $"`{account.DiscordID}`", false);
                    accountEmbed.AddField($"**Steam ID**", $"***You need to finish setting up your account***", false);
                    accountEmbed.AddField($"**Private Profile**", $"`{account.PrivateProfile}`", false);
                    accountEmbed.AddField($"**Account Permission**", $"`{account.Type}`", false);
                }
            }
            else
            {
                await ctx.Channel.SendMessageAsync($"This command can only be run within a DM channel or you do not have an account setup.").ConfigureAwait(false);
                return;
            }
        }

        [Command("-create")]
        [Aliases("-c")]
        [Description("Creates a package account. This command *must* be ran in order to get started with an account on Bloon.info")]
        public async Task CreatePackageAccount(CommandContext ctx)
        {
            if (this.CheckBasics(ctx) == true)
            {
                DiscordEmbedBuilder accountEmbed = new DiscordEmbedBuilder
                {
                    Footer = new DiscordEmbedBuilder.EmbedFooter
                    {
                        IconUrl = DiscordEmoji.FromGuildEmote(ctx.Client, ManageRoleEmojis.Warning).Url,
                        Text = "Package Account Verification and Disclaimer",
                    },
                    Color = new DiscordColor(52, 114, 53),
                    Timestamp = DateTime.UtcNow,
                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                    {
                        Url = ctx.User.AvatarUrl,
                    },
                    Description = "**Conditions of Account Creation**\n By running this command you've agreed to the terms and conditions (if any really). See the URL in the footer to see these terms. If you have any questions or would like to revert account creation, please contact bot admins.",
                };

                DateTime accountCreated = DateTime.Now;
                accountEmbed.AddField($"**Username**", $"`{ctx.User.Id}`", true);
                accountEmbed.AddField($"**Website**", $"[**Login Here**](https://bloon.info/login.php)", true);

                await this.accountService.AddAsync(ctx.User, accountCreated).ConfigureAwait(false);

                await ctx.Channel.SendMessageAsync(
                    "This message contains *your* **private** information. " +
                    "Pin this message for quick access to these details. " +
                    "We are not responsible for stolen accounts due to your lack of privacy.", embed: accountEmbed.Build()).ConfigureAwait(false);
            }
            else
            {
                await ctx.Channel.SendMessageAsync($"This command may only be ran within a DM or you already have an account.").ConfigureAwait(false);
            }
        }

        private bool CheckBasics(CommandContext ctx)
        {
            if (ctx.Channel.IsPrivate && this.accountService.FindAccount(ctx.User.Id) != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
