namespace Bloon.Variables
{
    public static class Guilds
    {
        public const ulong Bloon = 196820438398140417;

        public static ulong Leanto { get; private set; } = 303248156311814144;

        public static ulong Foxhound { get; private set; } = 126321177504382984;

        public static ulong SBG { get; private set; } = 103933666417217536;

        public static void MockFakeStub()
        {
            SBG = Bloon;
            Leanto = Bloon;
            Foxhound = Bloon;
        }
    }
}
