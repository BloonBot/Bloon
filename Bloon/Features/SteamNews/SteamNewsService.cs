namespace Bloon.Features.SteamNews
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Core.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json.Linq;
    using Serilog;

    public class SteamNewsService : ISocialService<SteamNewsPost>
    {
        private static readonly Regex BBCodeRegex = new Regex("\\[\\/?.+?\\]", RegexOptions.Compiled);
        private static readonly Regex ImageRegex = new Regex(@"({STEAM_CLAN_IMAGE}\/\d+\/\w+\.\w+)", RegexOptions.Compiled);
        private static readonly Regex SteamClanImageRegex = new Regex(@"{STEAM_CLAN(?:_LOC)?_IMAGE}", RegexOptions.Compiled);

        private readonly string apiKey;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly HttpClient httpClient;

        public SteamNewsService(IServiceScopeFactory scopeFactory, HttpClient httpClient)
        {
            this.apiKey = Environment.GetEnvironmentVariable("STEAM_API_KEY");
            this.scopeFactory = scopeFactory;
            this.httpClient = httpClient;
        }

        public async Task<SteamNewsPost> GetLatestAsync(string argument = null)
        {
            string responseRaw = string.Empty;

            JArray posts;

            try
            {
                responseRaw = await this.httpClient.GetStringAsync(new Uri($"https://api.steampowered.com/ISteamNews/GetNewsForApp/v1?appid=518150&key={this.apiKey}&count=1&maxlength=500"));
                JObject responseJson = JObject.Parse(responseRaw);
                posts = responseJson.SelectToken("appnews.newsitems.newsitem") as JArray;
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, "Unable to get latest Steam posts");
                return null;
            }
            catch (JsonException e)
            {
                Log.Error(e, "Unable to parse latest Steam posts\n{Response}", responseRaw);
                return null;
            }

            JObject jPost = posts[0] as JObject;

            Match imageMatch = ImageRegex.Match(jPost["contents"].ToString());

            SteamNewsPost post = new SteamNewsPost()
            {
                UID = jPost["gid"].ToString(),
                Title = jPost["title"].ToString(),
                Author = jPost["author"].ToString(),
                Description = ImageRegex.Replace(BBCodeRegex.Replace(jPost["contents"].ToString(), string.Empty), string.Empty),
                ImageUrl = imageMatch.Success
                        ? new Uri(SteamClanImageRegex.Replace(imageMatch.Value, "https://steamcdn-a.akamaihd.net/steamcommunity/public/images/clans"))
                        : null,
                Timestamp = DateTime.UnixEpoch.AddSeconds(jPost["date"].ToObject<int>()).ToUniversalTime(),
                Url = jPost["url"].ToObject<Uri>(),
            };

            return post;
        }

        public async Task<bool> TryStoreNewAsync(SteamNewsPost post)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            if (db.SteamNewsPosts.Any(x => x.UID == post.UID))
            {
                Log.Information("No new Steam posts");
                return false;
            }

            Log.Information("[STEAM] New POST!");

            db.SteamNewsPosts.Add(post);
            await db.SaveChangesAsync();

            return true;
        }
    }
}
