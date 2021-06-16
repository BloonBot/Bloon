namespace Bloon.Features.Workshop
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Core.Services;
    using Bloon.Features.IntruderBackend.Agents;
    using Bloon.Features.Workshop.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Serilog;

    public class WorkshopService : ISocialService<SocialItemWorkshopMap>
    {
        private static readonly Regex BBCodeRegex = new Regex("\\[\\/?.+?\\]", RegexOptions.Compiled);

        private readonly string apiKey;
        private readonly AgentService agentService;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly HttpClient httpClient;

        public WorkshopService(IServiceScopeFactory scopeFactory, HttpClient httpClient, AgentService agentService)
        {
            this.apiKey = Environment.GetEnvironmentVariable("STEAM_API_KEY");
            this.scopeFactory = scopeFactory;
            this.httpClient = httpClient;
            this.agentService = agentService;
        }

        /// <summary>
        /// Deprecated in favor of GetRecentlyUpdatedOrAddedAsync.
        /// </summary>
        /// <param name="argument">Ignored.</param>
        /// <returns>SocialItemWorkshop.</returns>
        public async Task<SocialItemWorkshopMap> GetLatestAsync(string argument = null)
        {
            string responseRaw = string.Empty;

            JObject jMap;

            try
            {
                responseRaw = await this.httpClient.GetStringAsync(new Uri($"https://api.steampowered.com/IPublishedFileService/QueryFiles/v1/?key={this.apiKey}&query_type=1&page=1&numperpage=1&appid=518150&return_previews=true&return_short_description=true"));
                JObject responseJson = JObject.Parse(responseRaw);
                jMap = responseJson.SelectToken("response.publishedfiledetails[0]") as JObject;
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, "Unable to get latest workshop map");
                return null;
            }
            catch (System.Text.Json.JsonException e)
            {
                Log.Error(e, "Unable to parse workshop map\n{Response}", responseRaw);
                return null;
            }

            if (jMap == null)
            {
                Log.Information("Unusable Steam response: {Response}", responseRaw);
                return null;
            }

            SocialItemWorkshopMap map = new SocialItemWorkshopMap()
            {
                UID = jMap["publishedfileid"].ToString(),
                Title = jMap["title"].ToString(),
                Author = jMap["creator"].ToString(),
                Description = BBCodeRegex.Replace(jMap["short_description"].ToString(), string.Empty),
                ThumbnailUrl = new Uri(jMap["preview_url"].ToString()),
                Timestamp = DateTime.UnixEpoch.AddSeconds(jMap["time_created"].ToObject<int>()),
            };

            map.Creator = await this.GetWorkshopMapCreator(jMap["creator"].ToObject<ulong>());

            Log.Debug($"[WORKSHOP]: {map.Timestamp.ToString(CultureInfo.InvariantCulture)}");

            return map;
        }

        public async Task<List<WorkshopMap>> GetRecentlyUpdatedOrAddedAsync()
        {
            List<WorkshopMap> workshopMaps = await this.GetAllMapsAsync();

            List<WorkshopMap> updatedMaps = new List<WorkshopMap>();

            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();
            foreach (WorkshopMap map in workshopMaps)
            {
                map.TimeCreated = UnixTimeStampToDateTime(Convert.ToDouble(map.UploadDate));
                map.TimeUpdated = UnixTimeStampToDateTime(Convert.ToDouble(map.MapUpdated));
                map.CreatorSteamID = Convert.ToUInt64(map.APICreator, CultureInfo.CurrentCulture);

                // if we have the map already stored in db, check if there is an update.
                if (db.WorkshopMaps.Any(x => x.FileID == map.FileID))
                {
                    // Updated map
                    WorkshopMap dbWorkshopMap = db.WorkshopMaps.Where(x => x.FileID == map.FileID).FirstOrDefault();
                    if (dbWorkshopMap.TimeUpdated.Date > map.TimeUpdated)
                    {
                        updatedMaps.Add(map);
                        db.WorkshopMaps.Update(map);
                    }

                    await db.SaveChangesAsync();
                }
                else
                {
                    // New map
                    updatedMaps.Add(map);
                    db.WorkshopMaps.Add(map);
                    Log.Information("[WORKSHOP] Stored new map in database.");
                    await db.SaveChangesAsync();
                }
            }

            return updatedMaps;
        }

        public async Task<List<WorkshopMap>> GetAllMapsAsync()
        {
            JToken response = await this.QueryAllMaps();
            if (response == null)
            {
                return null;
            }

            WorkshopRootObject rootObject = JsonConvert.DeserializeObject<WorkshopRootObject>(response.ToString());

            List<WorkshopMap> maps = new List<WorkshopMap>();

            if (rootObject == null || rootObject.Data == null || rootObject.Data.WorkshopMaps == null)
            {
                return maps;
            }

            foreach (WorkshopMap map in rootObject.Data.WorkshopMaps)
            {
                maps.Add(map);
            }

            int i = 1;
            while (rootObject.Data.Total != maps.Count)
            {
                i++;
                response = await this.QueryAllMaps(i);

                if (response == null)
                {
                    break;
                }

                rootObject = JsonConvert.DeserializeObject<WorkshopRootObject>(response.ToString());

                if (rootObject.Data?.WorkshopMaps == null)
                {
                    break;
                }

                foreach (WorkshopMap map in rootObject.Data.WorkshopMaps)
                {
                    maps.Add(map);
                }
            }

            return maps;
        }

        public async Task<List<WorkshopMap>> GetMapsFromDBAsync()
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();
            List<WorkshopMap> workshopMaps = new List<WorkshopMap>();

            workshopMaps = await db.WorkshopMaps.OrderByDescending(x => x.Favorited).ToListAsync();

            return workshopMaps;
        }

        public async Task StoreAllMaps(List<WorkshopMap> maps)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();
            foreach (WorkshopMap map in maps)
            {
                map.TimeCreated = UnixTimeStampToDateTime(Convert.ToDouble(map.UploadDate));
                map.TimeUpdated = UnixTimeStampToDateTime(Convert.ToDouble(map.MapUpdated));
                map.CreatorSteamID = Convert.ToUInt64(map.APICreator, CultureInfo.CurrentCulture);
                if (db.WorkshopMaps.Any(x => x.FileID == map.FileID))
                {
                    db.WorkshopMaps.Update(map);
                    Log.Information("[WORKSHOP] Updated Workshop Map stored in database.");
                }
                else
                {
                    db.WorkshopMaps.Add(map);
                    Log.Information("[WORKSHOP] Stored new map in database.");
                }
            }

            await db.SaveChangesAsync();
        }

        public async Task<bool> TryStoreNewAsync(SocialItemWorkshopMap map)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using BloonContext db = scope.ServiceProvider.GetRequiredService<BloonContext>();
            if (db.SocialItemWorkshopMap.Any(x => x.UID == map.UID))
            {
                Log.Information("No new Workshop maps");
                return false;
            }

            Log.Information("[WORKSHOP] New Map!");

            db.SocialItemWorkshopMap.Add(map);
            await db.SaveChangesAsync();

            return true;
        }

        public async Task<string> GetDBWorkshopMapCreator(ulong id)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();

            AgentsDB agent = new AgentsDB();

            if (this.agentService.CheckDBAgent(id))
            {
                agent = await this.agentService.GetDBAgentAsync(id);
            }
            else
            {
                await this.agentService.StoreAgentDBAsync(id);
                agent = await this.agentService.GetDBAgentAsync(id);
            }

            return agent.Name;
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        private async Task<JToken> QueryAllMaps(int page = 1)
        {
            try
            {
                string rawJson = await this.httpClient.GetStringAsync(new Uri($"https://api.steampowered.com/IPublishedFileService/QueryFiles/v1/?key={this.apiKey}&page={page}&numperpage=100&appid=518150&return_previews=true&return_short_description=true"));
                return string.IsNullOrEmpty(rawJson) ? null : JToken.Parse(rawJson);
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, "Unable to get wiki article");
                return null;
            }
        }

        private async Task<WorkshopMapCreator> GetWorkshopMapCreator(ulong id)
        {
            string responseRaw = string.Empty;

            JObject player;

            try
            {
                responseRaw = await this.httpClient.GetStringAsync(new Uri($"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={this.apiKey}&steamids={id}"));
                JObject responseJson = JObject.Parse(responseRaw);
                player = responseJson.SelectToken("response.players[0]") as JObject;
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, "Unable to fetch profile for {SteamId}", id);
                return null;
            }
            catch (System.Text.Json.JsonException e)
            {
                Log.Error(e, "Unable to parse profile for {SteamId}\n{Response}", id, responseRaw);
                return null;
            }

            return new WorkshopMapCreator()
            {
                AvatarUrl = player["avatarfull"].ToObject<Uri>(),
                Name = player["personaname"].ToString(),
                SteamId = player["steamid"].ToObject<ulong>(),
            };
        }
    }
}
