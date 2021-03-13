namespace Bloon.Features.Workshop.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class WorkshopData
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("publishedfiledetails")]
        public List<WorkshopMap> WorkshopMaps { get; set; }
    }
}
