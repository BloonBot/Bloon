namespace Bloon.Features.Wiki.Models
{
    using System.Text.Json;
    using Newtonsoft.Json;

    public class RootAPIObject
    {
        [JsonProperty("query")]
        public Query Query { get; set; }
    }
}
