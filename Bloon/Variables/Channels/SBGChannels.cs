namespace Bloon.Variables.Channels
{
    /// <summary>
    /// SuperbossGames Discord Guild Channels.
    /// </summary>
    public static class SBGChannels
    {
        public static ulong ModAlerts { get; private set; } = 892873525688434788;

        public static ulong ModServerLogs { get; private set; } = 892873550535467018;

        public static ulong ModJoinLeaveLog { get; private set; } = 892873587281780826;

        public static ulong HowToModerate { get; private set; } = 892873682479894539;

        public static ulong ModerationCommands { get; private set; } = 892873710053261312;

        public static ulong ModerationActions { get; private set; } = 892873793817686066;

        public static ulong ModerationsEvidence { get; private set; } = 892873844359057428;

        public static ulong ModChat { get; private set; } = 892873876848132196;

        public static ulong Announcements { get; private set; } = 125962601866854400;

        public static ulong DevUpdates { get; private set; } = 630812800389742592;

        public static ulong WorkshopUpdates { get; private set; } = 630826888604155908;

        public static ulong WelcomeAgents { get; private set; } = 151815021805043712;

        public static ulong CurrentServerInfo { get; private set; } = 143546335910428672;

        public static ulong JoinSBA { get; private set; } = 556614381870514176;

        public static ulong SpecialEvents { get; private set; } = 705918938818543677;

        public static ulong IntruderCommunities { get; private set; } = 814924571367047169;

        public static ulong Bugs { get; private set; } = 106405837810995200;

        public static ulong Help { get; private set; } = 306213064934424576;

        public static ulong General { get; private set; } = 103933666417217536;

        public static ulong CliffsideRework { get; private set; } = 822945681403805756;

        public static ulong PicsNVids { get; private set; } = 268062505761243137;

        public static ulong Mapmaker { get; private set; } = 141327816300953600;

        public static ulong MapMakerShowcase { get; private set; } = 630869030139592726;

        public static ulong TradingPost { get; private set; } = 813493071479701544;

        public static ulong CompetitiveMatches { get; private set; } = 305788366467760131;

        public static ulong AUG { get; private set; } = 103949138822963200;

        public static ulong SecretBaseAlpha { get; private set; } = 415199135583567882;

        public static ulong Wiki { get; private set; } = 154636594949652480;

        public static ulong LifeTheGame { get; private set; } = 141329672314028032;

        public static ulong Music { get; private set; } = 141328228567613441;

        public static ulong Bloonside { get; private set; } = 450731463286063104;

        public static ulong Offtopic { get; private set; } = 122802899905413120;

        public static ulong RulesAndInfo { get; private set; } = 892796013759303760;

        public static void MockFakeStub()
        {
            CurrentServerInfo = BloonChannels.CommandCentre;
            Help = BloonChannels.Ground0;
            Bugs = BloonChannels.CommandCentre;
            General = BloonChannels.Ground0;
            PicsNVids = BloonChannels.CommandCentre;
            AUG = BloonChannels.CommandCentre;
            SecretBaseAlpha = BloonChannels.Ground0;
            Wiki = BloonChannels.CommandCentre;
            Bloonside = BloonChannels.CommandCentre;
            MapMakerShowcase = BloonChannels.CommandCentre;
        }
    }
}
