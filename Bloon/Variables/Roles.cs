namespace Bloon.Variables
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Roles
    {
        public static void MockFakeStub()
        {
            SBG.MockFakeStub();
        }

        /// <summary>
        /// These roles are located within Bloon's development discord.
        /// Use these when attempting to do something with the roles within
        /// SBG. It'll help keep things lookin' clean for the people who
        /// use the front end of the bot.
        /// </summary>
        public static class Mock
        {
            public const ulong Admin = 223022425351127040;
            public const ulong All = 377137420417761292;
            public const ulong Muted = 889916846948614174;
            public const ulong KidsTable = 377137352746729472;
            public const ulong LookingToPlay = 889955578313605130;
        }

        /// <summary>
        /// SuperbossGames Discord Guild Roles.
        /// </summary>
        public static class SBG
        {
            public static ulong Developer { get; set; } = 103940914665242624;

            public static ulong LookingToPlay { get; private set; } = 258397123383525377;

            public static ulong Mod { get; private set; } = 122865698123808768;

            public static ulong NowPlaying { get; private set; } = 340487304651210773;

            public static ulong NowStreaming { get; private set; } = 293165177551978497;

            public static ulong SBA { get; private set; } = 556941912871796736;

            public static ulong Muted { get; private set; } = 645354840989630506;

            public static ulong Agent { get; set; } = 892781476414910464;

            public static ulong News { get; private set; } = 892798225034145843;

            public static ulong Nerds { get; private set; } = 892781549794254848;

            public static ulong AUG { get; private set; } = 103941588178202624;

            public static void MockFakeStub()
            {
                Developer = Mock.Admin;
                LookingToPlay = Mock.LookingToPlay;
                Mod = Mock.Admin;
                NowPlaying = Mock.All;
                NowStreaming = Mock.All;
                SBA = Mock.All;
                Muted = Mock.Muted;
                Agent = Mock.All;
            }
        }
    }
}
