using System;

namespace Bloon.Features.Wiki.Models
{
    public class Revision
    {
        public int revid { get; set; }

        public int parentid { get; set; }

        public string minor { get; set; }

        public string user { get; set; }

        public DateTime timestamp { get; set; }

        public string comment { get; set; }
    }

}
