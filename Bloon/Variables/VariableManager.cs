namespace Bloon.Variables
{
    using Bloon.Variables;
    using DSharpPlus;
    using Serilog;

    public static class VariableManager
    {
        public static void ApplyVariableScopes(DiscordClient dClient)
        {
            bool isDev = dClient.CurrentUser.Id == Users.Intruder;

            if (isDev)
            {
                Log.Warning("Intruder account detected, overriding SBG variables");
                Channels.MockFakeStub();
                Emojis.MockFakeStub();
                Guilds.MockFakeStub();
                Roles.MockFakeStub();
                Users.MockFakeStub();
            }
        }
    }
}
