namespace Bloon.Features.Youtube
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using Bloon.Core.Database;
    using Bloon.Core.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json.Linq;
    using Serilog;

    public class YouTubeService : ISocialService<YouTubeVideo>
    {
        private readonly string apiKey;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly HttpClient httpClient;

        public YouTubeService(IServiceScopeFactory scopeFactory, HttpClient httpClient)
        {
            this.apiKey = Environment.GetEnvironmentVariable("YOUTUBE_API_KEY");
            this.scopeFactory = scopeFactory;
            this.httpClient = httpClient;
        }

        public async Task<YouTubeVideo> GetLatestAsync(string argument = null)
        {
            string rawJson;

            try
            {
                rawJson = await this.httpClient.GetStringAsync(new Uri($"https://www.googleapis.com/youtube/v3/search?key={this.apiKey}&channelId=UCuxq1O0Giy8ZK67WmuD_HkA&part=snippet,id&order=date&maxResults=1"));
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, "Unable to get latest youtube video");
                return null;
            }

            if (string.IsNullOrEmpty(rawJson))
            {
                return null;
            }

            JObject jObject = JObject.Parse(rawJson);
            JToken yVideo = jObject?["items"]?[0];

            if (yVideo == null || yVideo["id"]["videoId"] == null)
            {
                return null;
            }

            YouTubeVideo video = new YouTubeVideo()
            {
                UID = yVideo["id"]["videoId"].ToString(),
                Title = HttpUtility.HtmlDecode(yVideo["snippet"]["title"].ToString()),
                Author = "Superboss Games",
                Description = yVideo["snippet"]["description"].ToString(),
                ThumbnailUrl = yVideo["snippet"]["thumbnails"]["medium"]["url"].ToString(),
                Timestamp = DateTime.Parse(yVideo["snippet"]["publishedAt"].ToString(), CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal),
            };

            Log.Debug($"[YOUTUBE]: {video.Timestamp.ToString(CultureInfo.InvariantCulture)}");

            return video;
        }

        public async Task<bool> TryStoreNewAsync(YouTubeVideo video)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            if (db.YoutubeVideos.Any(x => x.UID == video.UID))
            {
                Log.Information("No new YouTube videos");
                return false;
            }

            Log.Information("[YOUTUBE] New Video!");

            db.YoutubeVideos.Add(video);
            await db.SaveChangesAsync();

            return true;
        }
    }
}
