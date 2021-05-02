namespace Bloon.Core.Discord
{
    using System;
    using System.Net.WebSockets;
    using System.Threading.Tasks;
    using Bloon.Core.Commands;
    using Bloon.Core.Database;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using Bloon.Variables.Emojis;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Exceptions;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    public class Bot : Feature
    {
        private readonly IServiceProvider provider;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ActivityManager activityManager;
        private readonly BloonLog bloonLog;
        private readonly DiscordClient dClient;
        private CommandsNextExtension cNext;

        public Bot(IServiceProvider provider, IServiceScopeFactory scopeFactory, ActivityManager activityManager, DiscordClient dClient, BloonLog bloonLog)
        {
            this.activityManager = activityManager;
            this.dClient = dClient;
            this.provider = provider;
            this.scopeFactory = scopeFactory;
            this.bloonLog = bloonLog;
        }

        public static bool Ready { get; private set; }

        public static WebSocketState SocketState { get; private set; }

        public override string Name => "Bloon";

        public override string Description => "Wait what, are we enabling and disabling the bot from here too? What the hell?! Oh nvm, turns out we don't.";

        public override bool Protected => true;

        public override Task Initialize()
        {
            this.cNext = this.dClient.UseCommandsNext(new CommandsNextConfiguration
            {
                Services = this.provider,
                StringPrefixes = Environment.GetEnvironmentVariable("COMMAND_PREFIXES").Split(","),
            });

            AppDomain.CurrentDomain.ProcessExit += this.OnShutdown;

            return base.Initialize();
        }

        public override async Task Enable()
        {
            this.dClient.GuildAvailable += this.OnGuildAvailable;
            this.dClient.SocketOpened += this.OnSocketOpened;
            this.dClient.SocketClosed += this.OnSocketClosed;
            this.dClient.SocketErrored += this.OnSocketErrored;
            this.dClient.Ready += this.OnReady;

            this.cNext.CommandErrored += this.OnCommandErroredAsync;
            this.cNext.CommandExecuted += this.OnCommandExecuted;

            this.cNext.RegisterCommands<GeneralCommands>();
            this.cNext.RegisterCommands<OwnerCommands>();

            await this.dClient.InitializeAsync();
            await this.dClient.ConnectAsync();
        }

        private Task OnGuildAvailable(DiscordClient dClient, GuildCreateEventArgs args)
        {
            if (args.Guild.Id != Guilds.SBG)
            {
                return Task.CompletedTask;
            }

            return args.Guild.RequestMembersAsync();
        }

        private Task OnReady(DiscordClient dClient, ReadyEventArgs args)
        {
            Ready = true;
            return this.activityManager.ResetActivityAsync();
        }

        private Task OnSocketClosed(DiscordClient dClient, SocketCloseEventArgs args)
        {
            SocketState = WebSocketState.Closed;
            return Task.CompletedTask;
        }

        private Task OnSocketErrored(DiscordClient dClient, SocketErrorEventArgs args)
        {
            SocketState = WebSocketState.Closed;
            Log.Error(args.Exception, "Socket errored");
            return Task.CompletedTask;
        }

        private Task OnSocketOpened(DiscordClient dClient, SocketEventArgs args)
        {
            SocketState = WebSocketState.Open;
            VariableManager.ApplyVariableScopes(this.dClient);
            return Task.CompletedTask;
        }

        private async Task OnCommandErroredAsync(CommandsNextExtension cNext, CommandErrorEventArgs args)
        {
            if (args.Exception is ChecksFailedException)
            {
                await args.Context.Message.CreateReactionAsync(DiscordEmoji.FromName(this.dClient, ":underage:"));
                return;
            }
            else if (args.Exception is CommandNotFoundException)
            {
                if (!(args.Context.Message.Content.Length > 1 && args.Context.Message.Content[0] == args.Context.Message.Content[1]))
                {
                    await args.Context.RespondAsync($"'{args.Context.Message.Content.Split(' ')[0]}' is not a known command. See '.help'");
                }

                return;
            }

            await args.Context.Message.CreateReactionAsync(DiscordEmoji.FromName(this.dClient, ":interrobang:"));
            Log.Error(args.Exception, $"Command '{args.Context.Message.Content}' errored");
            this.bloonLog.Error($"`{args.Context.User.Username}` ran `{args.Context.Message.Content}` in **[{args.Context.Guild?.Name ?? "DM"} - {args.Context.Channel.Name}]**: {args.Exception.Message}");
        }

        private async Task OnCommandExecuted(CommandsNextExtension cNext, CommandExecutionEventArgs args)
        {
            string logMessage = $"`{args.Context.User.Username}` ran `{args.Context.Message.Content}` in **[{(args.Context.Guild != null ? $"{args.Context.Guild.Name} - {args.Context.Channel.Name}" : "DM")}]**";
            Log.Debug(logMessage);
            this.bloonLog.Information(LogConsole.Commands, CommandEmojis.Run, logMessage);

            using IServiceScope scope = this.scopeFactory.CreateScope();
            using AnalyticsContext db = scope.ServiceProvider.GetRequiredService<AnalyticsContext>();
            db.Commands.Add(new Analytics.Commands()
            {
                Command = args.Context.Message.Content.Substring(1),
                Guild = args.Context.Guild != null ? args.Context.Guild.Id : ulong.MinValue,
                Channel = args.Context.Channel != null ? args.Context.Channel.Id : ulong.MinValue,
                UserId = args.Context.User.Id,
                Link = args.Context.Guild != null ? $"https://discord.com/channels/{args.Context.Guild.Id}/{args.Context.Channel.Id}/{args.Context.User.Id}" : "DM",
                Timestamp = DateTime.Now,
            });

            await db.SaveChangesAsync();

            // Do we need this?
            return;
        }

        private void OnShutdown(object sender, EventArgs args)
        {
            this.dClient.DisconnectAsync();
        }
    }
}
