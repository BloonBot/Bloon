namespace Bloon.Features.RandomResponses
{
    using System;
    using System.Threading.Tasks;
    using Bloon.Variables;
    using DSharpPlus;
    using DSharpPlus.Entities;
    using DSharpPlus.EventArgs;

    public class MessageEvents
    {
        private const string TableFlipped = "(╯°□°）╯︵ ┻━┻";
        private const string TableUnflipped = "┬─┬ ノ( ゜-゜ノ)";

        private readonly DiscordClient dClient;

        public MessageEvents(DiscordClient dClient)
        {
            this.dClient = dClient;
        }

        public void Register()
        {
            this.dClient.MessageCreated += this.OnMessageCreatedAsync;
        }

        public void Unregister()
        {
            this.dClient.MessageCreated -= this.OnMessageCreatedAsync;
        }

        /// <summary>
        /// you know what this does.
        /// </summary>
        /// <param name="message">there shouldn't be a questiona bout this.</param>
        /// <returns>you know what this returns.</returns>
        private static async Task FlipTables(DiscordMessage message)
        {
            bool flipped = message.Content.Contains(TableFlipped, StringComparison.Ordinal);
            bool unflipped = message.Content.Contains(TableUnflipped, StringComparison.Ordinal);

            // Nothing to flip
            if (!flipped && !unflipped)
            {
                return;
            }

            Random random = new Random();
            int randomValue = random.Next(0, 10);

            // 10% chance of flipping out...I mean flipping tables
            if (randomValue == 0)
            {
                await message.Channel.SendMessageAsync(flipped ? TableUnflipped : TableFlipped);
            }
        }

        private async Task OnMessageCreatedAsync(DiscordClient dClient, MessageCreateEventArgs args)
        {
            if (args.Author.IsBot)
            {
                return;
            }

            // Respond to Rob
            await this.RespondToRobAsync(args.Message);

            // (Un)Flip tables
            await FlipTables(args.Message);
        }

        /// <summary>
        /// DEVELOPERS HATE HIM.
        /// </summary>
        /// <param name="message">Received Discord Message.</param>
        /// <returns>Task.</returns>
        private async Task RespondToRobAsync(DiscordMessage message)
        {
            if (message.Author.Id != Users.RobStorm)
            {
                return;
            }

            if (message.Content.Contains("csbutt", StringComparison.Ordinal) || message.Content.Contains("csbutts", StringComparison.Ordinal))
            {
                await message.CreateReactionAsync(DiscordEmoji.FromName(this.dClient, ":fire:"));
            }
        }
    }
}
