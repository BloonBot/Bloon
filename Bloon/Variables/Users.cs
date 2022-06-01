namespace Bloon.Variables
{
    /// <summary>
    /// This class is for users and their discord IDs that are referenced frequently in bloon
    /// They're return ulongs.
    /// </summary>
    public static class Users
    {
        public const ulong DukeofSussex = 244407876683169792;
        public const ulong Intruder = 296371559810203649;
        public const ulong RobStorm = 103932826268766208;
        public const ulong Ruby = 103967428408512512;

        public static ulong Bloon { get; private set; } = 225677282709209090;

        public static void MockFakeStub()
        {
            Bloon = Intruder;
        }
    }
}
