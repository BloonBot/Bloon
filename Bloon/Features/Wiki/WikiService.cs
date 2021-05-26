namespace Bloon.Features.Wiki
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using Bloon.Core.Database;
    using Bloon.Core.Services;
    using Bloon.Features.Wiki.Models;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Serilog;

    public class WikiService : ISocialService<WikiArticle>
    {
        private const string BaseUrl = "https://wiki.superbossgames.com/wiki/api.php?";

        // Last article that was edited
        private const string LatestRevisionParams = "action=query&format=json&list=recentchanges&rcprop=title|ids|sizes|flags|user|timestamp&rclimit=1";

        // Last 10 recent changes
        private const string RecentChangesParams = "action=query&list=recentchanges&rcprop=title|ids|sizes|flags|user|timestamp&rclimit=10&format=json";

        // Last 35 Recent Changes
        private const string RecentChangesthreefive = "action=query&list=recentchanges&rcprop=title|ids|sizes|flags|user|timestamp&rclimit=35&format=json";

        private const string RevisionParams = "action=query&format=json&prop=revisions&rvprop=timestamp";

        private const string ActiveUsers = "action=query&list=allusers&auactiveusers&format=json";

        private const string SearchParams = "action=parse&format=json&prop=wikitext|revid&redirects=true";

        private const string SuggestionParams = "action=opensearch";

        private readonly IServiceScopeFactory scopeFactory;
        private readonly HttpClient httpClient;

        public WikiService(IServiceScopeFactory scopeFactory, HttpClient httpClient)
        {
            this.scopeFactory = scopeFactory;
            this.httpClient = httpClient;
        }

        public async Task<List<string>> GetSuggestedPagesAsync(string searchTerm)
        {
            JToken response = await this.Query($"{SuggestionParams}&search={searchTerm}");

            if (response == null)
            {
                return new List<string>();
            }

            return response[1].Select(x => x.ToString()).ToList();
        }

        public async Task<List<RecentChange>> GetRecentChanges()
        {
            JToken response = await this.Query(RecentChangesParams);
            RootAPIObject rootObj = JsonConvert.DeserializeObject<RootAPIObject>(response.ToString());
            List<RecentChange> rc = new List<RecentChange>();
            foreach (RecentChange recent in rootObj.Query.RecentChanges)
            {
                rc.Add(recent);
            }

            return rc;
        }

        public async Task<List<WikiUser>> GetActiveUsers()
        {
            List<WikiUser> users = new List<WikiUser>();
            JToken response = await this.Query(ActiveUsers);

            if (response == null)
            {
                return null;
            }

            JToken jArticle = response["query"]["allusers"];

            for (int i = 0; i < jArticle.Count(); i++)
            {
                WikiUser activeUser = new WikiUser()
                {
                    Id = int.Parse(jArticle[i]["userid"].ToString(), CultureInfo.InvariantCulture),
                    Name = jArticle[i]["name"].ToString(),
                    CountRecentPosts = int.Parse(jArticle[i]["recentactions"].ToString(), CultureInfo.InvariantCulture),
                };
                users.Add(activeUser);
            }

            return users;
        }

        public async Task<List<RecentChange>> GetAllRecentChanges()
        {
            JToken response = await this.Query(RecentChangesthreefive);
            RootAPIObject rootObj = JsonConvert.DeserializeObject<RootAPIObject>(response.ToString());
            List<RecentChange> rc = new List<RecentChange>();
            foreach (RecentChange recent in rootObj.Query.RecentChanges)
            {
                rc.Add(recent);
            }

            return rc;
        }

        public async Task<WikiArticle> GetArticleAsync(string pageTitle)
        {
            Task<WikiArticle> section0 = this.GetArticleAsync(pageTitle, 0);
            Task<WikiArticle> section1 = this.GetArticleAsync(pageTitle, 1);

            WikiArticle[] articles = await Task.WhenAll(section0, section1);

            return articles[0] ?? articles[1];
        }

        public async Task<WikiArticle> GetLatestAsync(string argument = null)
        {
            JToken response = await this.Query(LatestRevisionParams);

            if (response == null)
            {
                return null;
            }

            JToken jArticle = response["query"]["recentchanges"][0];

            WikiArticle article = new WikiArticle()
            {
                UID = jArticle["rcid"].ToString(),
                Author = jArticle["user"].ToString(),
                NewPageSize = int.Parse(jArticle["newlen"].ToString(), CultureInfo.InvariantCulture),
                OldPageSize = int.Parse(jArticle["oldlen"].ToString(), CultureInfo.InvariantCulture),
                OldRevId = int.Parse(jArticle["old_revid"].ToString(), CultureInfo.InvariantCulture),
                PageId = int.Parse(jArticle["pageid"].ToString(), CultureInfo.InvariantCulture),
                RevId = int.Parse(jArticle["revid"].ToString(), CultureInfo.InvariantCulture),
                Timestamp = DateTime.Parse(jArticle["timestamp"].ToString(), CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal),
                Title = HttpUtility.HtmlDecode(jArticle["title"].ToString()),
                PostType = jArticle["type"].ToString(),
            };

            article.ByteDifference = article.NewPageSize - article.OldPageSize;

            Log.Debug($"[WIKI]: {article.Timestamp.ToString(CultureInfo.InvariantCulture)}");

            return article;
        }

        public async Task<bool> TryStoreNewAsync(WikiArticle entry)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            if (db.WikiArticles.Any(x => x.UID == entry.UID))
            {
                Log.Information("No new wiki posts found");
                return false;
            }

            Log.Information("[WIKI] Post Found!");

            db.WikiArticles.Add(entry);
            await db.SaveChangesAsync();

            return true;
        }

        private static bool IsValidArticle(JObject wikiObject)
        {

            // No error, has a revision id and wikitext
            return wikiObject["error"] == null
                && int.Parse(wikiObject["parse"]["revid"].ToString(), CultureInfo.InvariantCulture) > 0
                && !string.IsNullOrEmpty(wikiObject["parse"]["wikitext"]["*"].ToString());
        }

        private async Task<JToken> Query(string path)
        {
            try
            {
                string rawJson = await this.httpClient.GetStringAsync(new Uri(BaseUrl + path));
                return string.IsNullOrEmpty(rawJson) ? null : JToken.Parse(rawJson);
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, "Unable to get wiki article");
                return null;
            }
        }

        private async Task<WikiArticle> GetArticleAsync(string pageTitle, int section)
        {
            JToken response = await this.Query($"{SearchParams}&page={pageTitle}&section={section}");

            if (response == null || !IsValidArticle(response as JObject))
            {
                return null;
            }

            WikiArticle article = new WikiArticle()
            {
                Title = response["parse"]["title"].ToString(),
                Body = WikiUtils.ToMarkdown(response["parse"]["wikitext"]["*"].ToString()),
                Url = WikiUtils.GetUrlFromTitle(pageTitle),
                Timestamp = await this.GetLatestRevisionTimestamp(int.Parse(response["parse"]["revid"].ToString(), CultureInfo.InvariantCulture)),
            };

            return article;
        }

        private async Task<DateTime> GetLatestRevisionTimestamp(int revId)
        {
            JToken response = await this.Query($"{RevisionParams}&revids={revId}");

            if (response == null)
            {
                return DateTime.UnixEpoch;
            }

            JToken timestampToken = response.SelectToken("$..timestamp");

            if (timestampToken == null)
            {
                return DateTime.UnixEpoch;
            }

            return DateTime.Parse(timestampToken.ToString(), CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal);
        }
    }
}
