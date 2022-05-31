namespace Bloon.Variables
{
    public static class Guilds
    {
        public const ulong Bloon = 196820438398140417;

        public static ulong SBG { get; private set; } = 103933666417217536;

        public static void MockFakeStub()
        {
            SBG = Bloon;
        }
    }
}
