namespace Bloon.Features.IntruderBackend.News
{
    using Newtonsoft.Json;

    /// <summary>
    /// https://api.intruderfps.com/news .
    /// </summary>
    public class NewsObject
    {
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        public News[] Data { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("perPage")]
        public int EntriesPerPage { get; set; }
    }
}
