using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bloon.Features.Workshop.Models
{
    public class WorkshopRootObject
    {
        [JsonProperty("response")]
        public WorkshopData Data { get; set; }
    }
    public class WorkshopData
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("publishedfiledetails")]
        public WorkshopMap[] WorkshopMaps { get; set; }
    }
}
