namespace Bloon.Features.Workshop.Models
{
    using Newtonsoft.Json;

    public class WorkshopRootObject
    {
        [JsonProperty("response")]
        public WorkshopData Data { get; set; }
    }
}
