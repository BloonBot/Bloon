namespace Bloon.Core.Discord
{
    using System;
    using System.Net.WebSockets;
    using System.Threading.Tasks;
    using Bloon.Core.Commands;
    using Bloon.Core.Services;
    using Bloon.Variables;
    using Bloon.Variables.Emojis;
    using DSharpPlus;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Exceptions;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;
    using Serilog;

    public class Bot : Feature
    {
        private readonly IServiceProvider provider;
        private readonly ActivityManager activityManager;
        private readonly BloonLog bloonLog;
        private readonly DiscordClient dClient;
        private CommandsNextExtension cNext;

        public Bot(IServiceProvider provider, ActivityManager activityManager, DiscordClient dClient, BloonLog bloonLog)
        {
            this.activityManager = activityManager;
            this.dClient = dClient;
            this.provider = provider;
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

            await this.dClient.InitializeAsync().ConfigureAwait(false);
            await this.dClient.ConnectAsync().ConfigureAwait(false);
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
                await args.Context.Message.CreateReactionAsync(DiscordEmoji.FromName(this.dClient, ":underage:")).ConfigureAwait(false);
                return;
            }
            else if (args.Exception is CommandNotFoundException)
            {
                if (!(args.Context.Message.Content.Length > 1 && args.Context.Message.Content[0] == args.Context.Message.Content[1]))
                {
                    await args.Context.RespondAsync($"'{args.Context.Message.Content.Split(' ')[0]}' is not a known command. See '.help'").ConfigureAwait(false);
                }

                return;
            }

            Log.Error(args.Exception, $"Command '{args.Context.Message.Content}' errored");
            this.bloonLog.Error($"`{args.Context.User.Username}` ran `{args.Context.Message.Content}` in **[{args.Context.Guild?.Name ?? "DM"} - {args.Context.Channel.Name}]**: {args.Exception.Message}");
        }

        private Task OnCommandExecuted(CommandsNextExtension cNext, CommandExecutionEventArgs args)
        {
            string logMessage = $"`{args.Context.User.Username}` ran `{args.Context.Message.Content}` in **[{(args.Context.Guild != null ? $"{args.Context.Guild.Name} - {args.Context.Channel.Name}" : "DM")}]**";
            Log.Debug(logMessage);
            this.bloonLog.Information(LogConsole.Commands, CommandEmojis.Run, logMessage);
            return Task.CompletedTask;
        }

        private void OnShutdown(object sender, EventArgs args)
        {
            this.dClient.DisconnectAsync();
        }
    }
}
