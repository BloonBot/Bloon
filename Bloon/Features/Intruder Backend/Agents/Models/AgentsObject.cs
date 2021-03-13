namespace Bloon.Features.IntruderBackend.Agents
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    /// <summary>
    /// https://api.intruderfps.com/agents.
    /// </summary>
    [NotMapped]
    public class AgentsObject
    {
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        public List<Agent> Data { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("perPage")]
        public int EntriesPerPage { get; set; }
    }
}
