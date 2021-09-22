namespace Bloon.Variables.Roles
{
    /// <summary>
    /// SuperbossGames Discord Guild Roles.
    /// </summary>
    public static class SBGRoles
    {
        public static ulong Developer { get; set; } = 103940914665242624;

        public static ulong LookingToPlay { get; private set; } = 258397123383525377;

        public static ulong Mod { get; private set; } = 122865698123808768;

        public static ulong NowPlaying { get; private set; } = 340487304651210773;

        public static ulong NowStreaming { get; private set; } = 293165177551978497;

        public static ulong SBA { get; private set; } = 556941912871796736;

        public static void MockFakeStub()
        {
            Developer = MockRoles.Admin;
            LookingToPlay = MockRoles.LookingToPlay;
            Mod = MockRoles.Admin;
            NowPlaying = MockRoles.All;
            NowStreaming = MockRoles.All;
            SBA = MockRoles.All;
        }
    }
}
