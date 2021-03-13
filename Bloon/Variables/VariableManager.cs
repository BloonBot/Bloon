namespace Bloon.Variables
{
    using Bloon.Variables.Channels;
    using Bloon.Variables.Roles;
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
                BloonChannels.Settings = BloonChannels.SettingsDebug;
                Guilds.MockFakeStub();
                SBGChannels.MockFakeStub();
                SBGRoles.MockFakeStub();
            }
        }
    }
}
