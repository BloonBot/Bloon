namespace Bloon.Variables
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class Emojis
    {
        public static void MockFakeStub()
        {
            SBG.MockFakeStub();
        }

        public static class Browser
        {
            public const ulong Chrome = 288220315736342528;
            public const ulong Firefox = 288220355028582402;
        }

        public static class Command
        {
            public const ulong Run = 440596769928249354;
        }

        public static class DayNight
        {
            public const ulong Sunrise = 815294620693954620;
            public const ulong Sun = 815256286282776586;
            public const ulong Sunset = 815294621075898388;
            public const ulong Moonrise = 815294619968602123;
            public const ulong Moon = 815256286018797588;
            public const ulong Moonset = 815294620300607539;
        }

        public static class Event
        {
            public const ulong Join = 562386125256130572;
            public const ulong Leave = 562386124710871091;
            public const ulong Banned = 805838946868133938;
            public const ulong Edited = 805839239757168641;
        }

        public static class Feature
        {
            public const ulong ToggleOff = 768577752714182659;
            public const ulong ToggleOn = 768577752689147944;
        }

        public static class ManageRole
        {
            public const ulong Promotion = 440594409449324546;
            public const ulong Demotion = 440594409420095508;
            public const ulong Warning = 562386125105397770;
            public const ulong BloonMoji = 892799370372739072;
        }

        public static class Platform
        {
            public const ulong YouTube = 288084412158050304;
            public const ulong Twitter = 288089852736438273;
            public const ulong Steam = 288084279349608448;
            public const ulong Reddit = 288084240967401474;
            public const ulong Wiki = 587812105340846100;
            public const ulong Twitch = 655655324308471829;
            public const ulong Github = 891007467910217739;
            public const ulong Discord = 891008079892721725;
        }

        public static class RegionFlag
        {
            public const string Asia = ":flag_sg:"; // Asia (Singapore)
            public const string AU = ":flag_au:"; // Australia
            public const string EU = ":flag_eu:"; // Europe
            public const string JP = ":flag_jp:"; // Japan
            public const string RU = ":flag_ru:"; // Russia
            public const string SA = ":flag_br:"; // South America (Brazil)
            public const string US = ":flag_us:"; // United States
            public const string IN = ":flag_in:"; // India
            public const string KR = ":flag_kr:"; // Korea(s)?
            public const string CAE = ":flag_ca:"; // Canada I guess
        }

        public static class Reputation
        {
            public const ulong Up = 562344798414176282;
            public const ulong Down = 562344801496858644;
        }

        public static class SBG
        {
            public static ulong Bloon { get; private set; } = 244201882484998154;

            public static ulong Superboss { get; private set; } = 246666490449887232;

            public static void MockFakeStub()
            {
                Bloon = Command.Run;
                Superboss = Command.Run;
            }
        }

        public static class Server
        {
            public const ulong Verified = 408380944605773834;
            public const ulong Unofficial = 408685552024420367;
            public const ulong Tuning = 408379266649292810;
            public const ulong Password = 408379266527395850;
            public const ulong Official = 408379266481389588;
            public const ulong Players = 815296187249852457;
            public const ulong Map = 815431725303070760;
        }
    }
}
