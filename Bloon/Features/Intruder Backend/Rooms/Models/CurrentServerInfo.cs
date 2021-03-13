namespace Bloon.Features.IntruderBackend.Rooms
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bloon.Features.Doorman;
    using Newtonsoft.Json;

    /// <summary>
    /// Not used via api.intruderfps.com. This is a model for Bloon's internal use only as it creates the embed based off these details.
    /// </summary>
    [NotMapped]
    public class CurrentServerInfo
    {
        public List<Rooms> Rooms { get; set; }

        public int PlayerCount { get; set; }

        public int USPlayerCount { get; set; }

        public ulong USTOD { get; set; }

        public int USRoomCount { get; set; }

        public int EUPlayerCount { get; set; }

        public int EURoomCount { get; set; }

        public ulong EUTOD { get; set; }

        public int RUPlayerCount { get; set; }

        public int RURoomCount { get; set; }

        public ulong RUTOD { get; set; }

        public int AUPlayerCount { get; set; }

        public int AURoomCount { get; set; }

        public ulong AUTOD { get; set; }

        public int AsiaPlayerCount { get; set; }

        public int AsiaRoomCount { get; set; }
        public ulong ASTOD { get; set; }

        public int JPPlayerCount { get; set; }

        public int JPRoomCount { get; set; }

        public ulong JPTOD { get; set; }

        public int SAPlayerCount { get; set; }

        public int SARoomCount { get; set; }

        public ulong SATOD { get; set; }

        public int CAEPlayerCount { get; set; }

        public int CAERoomCount { get; set; }

        public ulong CAETOD { get; set; }

        public int KRPlayerCount { get; set; }

        public int KRRoomCount { get; set; }

        public ulong KRTOD { get; set; }

        public int INPlayerCount { get; set; }

        public int INRoomCount { get; set; }

        public ulong INTOD { get; set; }
    }
}
