namespace Bloon.Core.Discord
{
    using System.Linq;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.SlashCommands;

    public static class DiscordExtensions
    {
        /// <summary>
        /// How we can un-register commands on the fly.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="cNext">CommandsNext</param>
        public static void UnregisterCommands<T>(this CommandsNextExtension cNext)
        {
            cNext.UnregisterCommands(cNext
                .RegisteredCommands
                .Where(c => c.Value.Module.ModuleType == typeof(T))
                .Select(c => c.Value)
                .ToArray());
        }
    }
}
