namespace Bloon.Features.Helprace
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
    using Bloon.Utils;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json.Linq;
    using Serilog;

    public class HelpraceService : ISocialService<HelpracePost>
    {
        private const string HelpraceURL = "https://superbossgames.helprace.com/api/v1";

        private readonly IServiceScopeFactory scopeFactory;
        private readonly HttpClient httpClient;

        /// <summary>
        /// This list relates to the different types of helprace posts.
        /// </summary>
        private readonly List<string> permittedArguments = new List<string>()
        {
            "updates", "ideas", "knowledgebase", "praise", "problems", "questions", "all_topics",
        };

        public HelpraceService(IServiceScopeFactory scopeFactory, HttpClient httpClient)
        {
            this.scopeFactory = scopeFactory;
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Obtains the helprace data.
        /// </summary>
        /// <param name="argument">What kind of data do you want back from the helprace?.</param>
        /// <returns>Raw Json helprace data, stored in a string.</returns>
        public async Task<string> QueryHelprace(string argument)
        {
            try
            {
                return await this.httpClient.GetStringAsync(new Uri($"{HelpraceURL}/{argument}?sort_by=created&sort_order=desc&fields=id%2Ctitle%2Cbody%2Ccreated%2Cauthor%2Cvotes%2Cchannel")).ConfigureAwait(false);
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, "Failed to Query Helprace Data");
                return null;
            }
        }

        /// <summary>
        /// This will obtain the latest helprace post. It'll clean the HTML and truncate the body text down to 256 characters. Can be increased later.
        /// TODO: Add the truncate value to the DB so we can edit on the fly instead of hardcoded.
        /// </summary>
        /// <param name="argument">What kind of helprace post should we scrape?.</param>
        /// <returns>Helprace Entry.</returns>
        public async Task<HelpracePost> GetLatestAsync(string argument)
        {
            string rawHelprace;

            if (this.CheckArguments(argument) == true)
            {
                rawHelprace = await this.QueryHelprace(argument).ConfigureAwait(false);
            }
            else
            {
                Log.Debug($"Failed to obtain Helprace Post, check returned false/null");
                return null;
            }

            JObject jObject = JObject.Parse(rawHelprace);
            JToken jPost = jObject["topics"]["data"][0];

            HelpracePost post = new HelpracePost()
            {
                UID = jPost["id"].ToString(),
                Channel = jPost["channel"].ToString(),
                Title = HttpUtility.HtmlDecode(jPost["title"].ToString()),
                Body = Sanitize.RemoveBreaks(jPost["body"].ToString()),  // Remove the page breaks and fluff that can come with helprace entries.
                Timestamp = DateTime.Parse(jPost["created"].ToString(), CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal),
                Author = jPost["author"]["name"].ToString(),
            };
            post.Body = post.Body.Truncate(256);   // Truncate the body text so we don't run into an overflow error for the discord embed later.

            Log.Debug($"[HELPRACE]: {post.Timestamp.ToString(CultureInfo.InvariantCulture)}");
            return post;
        }

        /// <summary>
        /// Check to ensure that the argument we're passing around is valid.
        /// </summary>
        /// <param name="argument">The helprace argument that is stored in the PermittedArguments list.</param>
        /// <returns>true if the argument is within that list.</returns>
        public bool CheckArguments(string argument)
        {
            if (!this.permittedArguments.Contains(argument))
            {
                Log.Error($"Unknown helprace query argument: {argument}");
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Obtain a defined amount of topics from helprace given the particular argument.
        /// </summary>
        /// <param name="pageNumber">What kind of data do you want back from the helprace?.</param>
        /// <returns>An array of helprace posts.</returns>
        public async Task GetAllTopics(int pageNumber = 1)
        {
            string rawHelprace = await this.httpClient.GetStringAsync(new Uri($"{HelpraceURL}/all_topics?sort_by=created&sort_order=desc&fields=id%2Ctitle%2Cbody%2Ccreated%2Cauthor%2Cvotes%2Cchannel&per_page=100&page={pageNumber}")).ConfigureAwait(false);

            JObject jObject = JObject.Parse(rawHelprace);
            JArray jPosts = jObject["topics"]["data"] as JArray;

            for (int i = 0; i < jPosts.Count; i++)
            {
                HelpracePost post = new HelpracePost()
                {
                    Author = jPosts[i]["author"]["name"].ToString(),
                    Body = Sanitize.RemoveBreaks(jPosts[i]["body"].ToString().Truncate(256)),
                    Channel = jPosts[i]["channel"].ToString(),
                    UID = jPosts[i]["id"].ToString(),
                    Timestamp = DateTime.Parse(jPosts[i]["created"].ToString(), CultureInfo.CurrentCulture, DateTimeStyles.AdjustToUniversal),
                    Title = HttpUtility.HtmlDecode(jPosts[i]["title"].ToString()),
                };

                await this.TryStoreNewAsync(post).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Attempt to store the new helprace post in the DB. If entry exists with matching ID, don't store it.
        /// </summary>
        /// <param name="entry">A helprace post.</param>
        /// <returns>True or false if it stored it or not.</returns>
        public async Task<bool> TryStoreNewAsync(HelpracePost entry)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            if (db.HelpracePosts.Any(x => x.UID == entry.UID))
            {
                Log.Information("No new helprace posts");
                return false;
            }

            Log.Information("[HELPRACE] Found new post. Adding {0}. Author: {1}", entry.UID, entry.Author);

            db.HelpracePosts.Add(entry);
            await db.SaveChangesAsync().ConfigureAwait(false);

            return true;
        }
    }
}
