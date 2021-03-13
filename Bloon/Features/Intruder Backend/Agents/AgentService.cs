namespace Bloon.Features.IntruderBackend.Agents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Features.IntruderBackend.Levels;
    using Bloon.Features.PackageAccounts;
    using Bloon.Core.Discord;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Serilog;
    using Bloon.Commands;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.EntityFrameworkCore;

    public class AgentService
    {
        private const string BaseUrl = "https://api.intruderfps.com/agents";
        private const string StatsEndpoint = "/stats";
        private const string VotesEndpoint = "/votes";
        private const string MapsEndpoint = "/maps";

        public const string Q = "Q=";
        public const string OrderBy = "&OrderBy=";
        public const string OnlineOnly = "&OnlineOnly=true";
        public const string Page = "&Page=";
        public const string PerPage = "&PerPage=";

        private readonly IServiceScopeFactory scopeFactory;
        private readonly HttpClient httpClient;
        private readonly AccountService accountService;

        public AgentService(IServiceScopeFactory scopeFactory, HttpClient httpClient, AccountService accountService)
        {
            this.scopeFactory = scopeFactory;
            this.httpClient = httpClient;
            this.accountService = accountService;
        }

        /// <summary>
        /// For use in searching https://api.intruderfps.com/agents.
        /// </summary>
        /// <param name="q">Gets or sets the (partial) agent name or Steam ID filter.</param>
        /// <param name="orderBy">Gets or sets the result set ordering using property:direction.</param>
        /// <param name="onlineOnly">Gets or sets a value indicating whether only online agents should be retrieved.</param>
        /// <param name="page">Gets or sets the current pagination page.</param>
        /// <param name="perPage">Gets or sets the amount of entries per page.</param>
        /// <returns>All agents.</returns>
        public async Task<List<Agent>> GetAgents(string? q, string? orderBy, bool? onlineOnly, int? page, int? perPage)
        {
            JToken queryResponse = await this.QueryAgents(q, orderBy, onlineOnly, page, perPage).ConfigureAwait(false);

            AgentsObject agentsObject = JsonConvert.DeserializeObject<AgentsObject>(queryResponse.ToString());

            List<Agent> agents = new List<Agent>();

            foreach (Agent agent in agentsObject.Data)
            {
                agents.Add(agent);
            }

            return agents;
        }

        /// <summary>
        /// Searches the Intruder API for a single or list of agents.
        /// </summary>
        /// <param name="usernameOrID">Can be the Steam Username or Steam ID of an agent.</param>
        /// <returns>Lists of matched Agents.</returns>
        public async Task<List<Agent>> SearchAgent(string? usernameOrID)
        {
            JToken queryResponse = await this.QueryAgents(usernameOrID, null, null, null, null).ConfigureAwait(false);
            AgentsObject agentsObject = JsonConvert.DeserializeObject<AgentsObject>(queryResponse.ToString());
            List<Agent> agents = new List<Agent>();

            foreach (Agent agent in agentsObject.Data)
            {
                agents.Add(agent);
            }

            return agents;
        }

        /// <summary>
        /// Collects and stores the agent_history from all Package Account Users.
        /// </summary>
        /// <returns>Jack shit.</returns>
        public async Task ScrapeHistoricalData()
        {
            // Obtain all package accounts with their steam IDs
            List<PackageAccount> packageAccounts = this.accountService.QueryAccounts();

            // Convert SteamIDs from PackageAccounts to Agents using DB.
            List<Agent> agents = new List<Agent>();
            foreach (PackageAccount account in packageAccounts)
            {
                agents.Add(await this.GetAgentProfileAsync(account.SteamID).ConfigureAwait(false));
            }

            foreach (Agent dbAgent in agents)
            {
                // Collect Agent Stats
                AgentStats statsResponse = await this.GetAgentStatsAsync(dbAgent.SteamID).ConfigureAwait(false);

                // Collect Agent Votes
                List<AgentVotes> votesResponse = await this.QueryAgentVotes(dbAgent.SteamID).ConfigureAwait(false);

                // Collect Profile Information
                Agent agentResponse = await this.GetAgentProfileAsync(dbAgent.SteamID).ConfigureAwait(false);

                // If the player's latest lastUpdate value is greater than what we have stored in the database
                if (statsResponse.LastUpdate > dbAgent.LastUpdate && agentResponse.LoginCount > dbAgent.LoginCount)
                {
                    AgentHistory agentHistory = new AgentHistory()
                    {
                        SteamID = dbAgent.SteamID,
                        MatchesWon = statsResponse.MatchesWon,
                        MatchesLost = statsResponse.MatchesLost,
                        RoundsLost = statsResponse.RoundsLost,
                        RoundsTied = statsResponse.RoundsTied,
                        RoundsWonElim = statsResponse.RoundsWonElim,
                        RoundsWonCapture = statsResponse.RoundsWonCapture,
                        RoundsWonHack = statsResponse.RoundsWonHack,
                        RoundsWonTimer = statsResponse.RoundsWonTimer,
                        RoundsWonCustom = statsResponse.RoundsWonCustom,
                        TimePlayed = statsResponse.TimePlayed,
                        Kills = statsResponse.Kills,
                        TeamKills = statsResponse.TeamKills,
                        Deaths = statsResponse.Deaths,
                        Arrests = statsResponse.Arrests,
                        GotArrested = statsResponse.GotArrested,
                        Captures = statsResponse.Captures,
                        NetworkHacks = statsResponse.NetworkHacks,
                        Survivals = statsResponse.Survivals,
                        Suicides = statsResponse.Suicides,
                        Knockdowns = statsResponse.Knockdowns,
                        GotKnockedDown = statsResponse.GotKnockedDown,
                        TeamKnockdowns = statsResponse.TeamKnockdowns,
                        TeamDamage = statsResponse.TeamDamage,
                        LevelXP = statsResponse.LevelXP,
                        TotalXP = statsResponse.TotalXP,
                        Level = statsResponse.Level,
                        PositiveVotes = votesResponse.FirstOrDefault().PositiveVotes,
                        NegativeVotes = votesResponse.FirstOrDefault().NegativeVotes,
                        TotalVotes = votesResponse.FirstOrDefault().ReceivedVotes,
                        LoginCount = dbAgent.LoginCount,
                        LastLogin = dbAgent.LastLogin,
                        Timestamp = DateTime.Now,
                    };

                    // Store new Agent Stat History in DB
                    using IServiceScope scope = this.scopeFactory.CreateScope();
                    using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();
                    try
                    {
                        db.AgentHistory.Add(agentHistory);
                        await db.SaveChangesAsync().ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, $"Failed to store agent histoy in database. Agent: {agentHistory.ID}");
                    }

                    // Update Agent to include most recent XP.
                    //dbAgent.LastUpdate = DateTime.Now;
                    //dbAgent.XP = statsResponse.TotalXP;
                    //try
                    //{
                    //    db.Agents.Update(dbAgent);
                    //    await db.SaveChangesAsync().ConfigureAwait(false);
                    //}
                    //catch (Exception e)
                    //{
                    //    Log.Error(e, $"Failed to update agent details in database. Agent: {dbAgent.Name} | ID: {dbAgent.SteamID}");
                    //}
                }
            }
        }

        /// <summary>
        /// Collects and stores the agent_history from all Package Account Users.
        /// </summary>
        /// <returns>Jack shit.</returns>
        public async Task PopulateAgentTableAsync()
        {

            // Convert SteamIDs from PackageAccounts to Agents using DB.
            List<Agent> agents = await this.GetAllAgents(null, null, null, 101, 100).ConfigureAwait(false);

            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();

            foreach (Agent dbAgent in agents)
            {
                if (db.Agents.Where(x => x.SteamID == dbAgent.SteamID).AsNoTracking().FirstOrDefault() == null || db.Agents.Any(x => x.SteamID != dbAgent.SteamID) || db.Agents.Any(x => x.ID != dbAgent.IntruderID))
                {
                    // Collect Agent Stats
                    AgentStats statsResponse = await this.GetAgentStatsAsync(dbAgent.SteamID).ConfigureAwait(false);

                    // Collect Agent Votes
                    List<AgentVotes> votesResponse = await this.QueryAgentVotes(dbAgent.SteamID).ConfigureAwait(false);

                    AgentsDB agentsDB = new AgentsDB
                    {
                        // BEGIN PROFILE INFO
                        SteamID = dbAgent.SteamID,
                        SteamAvatar = dbAgent.AvatarURL,
                        ID = dbAgent.IntruderID,
                        Role = dbAgent.Role,
                        Name = dbAgent.Name,
                        LoginCount = dbAgent.LoginCount,
                        FirstLogin = dbAgent.FirstLogin,
                        LastLogin = dbAgent.LastLogin,
                        LastUpdate = dbAgent.LastUpdate,
                        // END PROFILE INFO
                        // BEGIN STATS INFO
                        MatchesWon = statsResponse.MatchesWon,
                        MatchesLost = statsResponse.MatchesLost,
                        RoundsLost = statsResponse.RoundsLost,
                        RoundsTied = statsResponse.RoundsTied,
                        RoundsWonElim = statsResponse.RoundsWonElim,
                        RoundsWonCapture = statsResponse.RoundsWonCapture,
                        RoundsWonHack = statsResponse.RoundsWonHack,
                        RoundsWonTimer = statsResponse.RoundsWonTimer,
                        RoundsWonCustom = statsResponse.RoundsWonCustom,
                        TimePlayed = statsResponse.TimePlayed,
                        Kills = statsResponse.Kills,
                        TeamKills = statsResponse.TeamKills,
                        Deaths = statsResponse.Deaths,
                        Arrests = statsResponse.Arrests,
                        GotArrested = statsResponse.GotArrested,
                        Captures = statsResponse.Captures,
                        Pickups = statsResponse.Pickups,
                        NetworkHacks = statsResponse.NetworkHacks,
                        Survivals = statsResponse.Survivals,
                        Suicides = statsResponse.Suicides,
                        Knockdowns = statsResponse.Knockdowns,
                        GotKnockedDown = statsResponse.GotKnockedDown,
                        TeamKnockdowns = statsResponse.TeamKnockdowns,
                        TeamDamage = statsResponse.TeamDamage,
                        Level = statsResponse.Level,
                        LevelXP = statsResponse.LevelXP,
                        LevelXPRequired = statsResponse.LevelXPRequired,
                        TotalXP = statsResponse.TotalXP,
                        // END STATS INFO
                        // BEGIN VOTES
                        PositiveVotes = votesResponse.ElementAt(0).PositiveVotes,
                        NegativeVotes = votesResponse.ElementAt(0).NegativeVotes,
                        TotalVotes = votesResponse.ElementAt(0).ReceivedVotes,
                        Timestamp = DateTime.Now,
                    };
                    try
                    {
                        db.Agents.Add(agentsDB);
                        await db.SaveChangesAsync().ConfigureAwait(false);
                    }
                    catch (DbUpdateException e1)
                    {
                        db.Agents.Update(agentsDB);
                        await db.SaveChangesAsync().ConfigureAwait(false);
                    }
                }
                else
                {
                    Console.WriteLine($"{dbAgent.Name} IS ALREADY STORED. INTRUDER ID: {dbAgent.IntruderID}");
                }
            }
        }

        /// <summary>
        /// Quries the Intruder API endpoint for agents.
        /// https://api.intruderfps.com/agents.
        /// </summary>
        /// <param name="q">Gets or sets the (partial) agent name or Steam ID filter.</param>
        /// <param name="orderBy">Gets or sets the result set ordering using property:direction.</param>
        /// <param name="onlineOnly">Gets or sets a value indicating whether only online agents should be retrieved.</param>
        /// <param name="page">Gets or sets the current pagination page.</param>
        /// <param name="perPage">Gets or sets the amount of entries per page.</param>
        /// <returns>JToken.</returns>
        private async Task<JToken> QueryAgents(string? q, string? orderBy, bool? onlineOnly, int? page, int? perPage)
        {
            // https://api.intruderfps.com/agents?
            // if i want numbers in my damn variables i'm gonna put numbers in my variables.
            StringBuilder urlBuilder3000 = new StringBuilder(BaseUrl);

            if (q != null)
            {
                // https://api.intruderfps.com/agents?Q=XXXXXXXXX
                urlBuilder3000.Append($"?Q={q}");
            }

            if (orderBy != null)
            {
                // https://api.intruderfps.com/agents?Q=XXXXXXXXX&OrderBy=XXXX:XXXX
                // https://api.intruderfps.com/agents?&OrderBy=XXXX:XXXX
                urlBuilder3000.Append($"?OrderBy={orderBy}");
            }

            if (onlineOnly != null)
            {
                // https://api.intruderfps.com/agents?Q=XXXXXXXXX&OrderBy=XXXX:XXXX&OnlineOnly=XXXXX
                // https://api.intruderfps.com/agents?&OnlineOnly=XXXXX
                urlBuilder3000.Append($"&OnlineOnly={onlineOnly}");
            }

            if (page != null)
            {
                if (page == 0)
                {
                    page = 1;
                }

                // https://api.intruderfps.com/agents?Q=XXXXXXXXX&OrderBy=XXXX:XXXX&OnlineOnly=XXXXX&Page=X
                // https://api.intruderfps.com/agents?&Page=X
                urlBuilder3000.Append($"?Page={page}");
            }

            if (perPage != null)
            {
                // if you try to query lower than 25 results per page, force it to 25 records per page.
                if (perPage < 25)
                {
                }

                // the limit of the API is 100.
                if (perPage > 100)
                {
                }

                // https://api.intruderfps.com/agents?Q=XXXXXXXXX&OrderBy=XXXX:XXXX&OnlineOnly=XXXXX&Page=X&PerPage=XX
                // https://api.intruderfps.com/agents?&PerPage=XX
                urlBuilder3000.Append($"&PerPage={perPage}");
            }

            // DEBUGGING
            Console.WriteLine(urlBuilder3000.ToString());

            try
            {
                string rawJson = await this.httpClient.GetStringAsync(new Uri(urlBuilder3000.ToString())).ConfigureAwait(false);
                return string.IsNullOrEmpty(rawJson) ? null : JToken.Parse(rawJson);
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, $"Failed to run a query for Agents. URL: {urlBuilder3000}");
                return null;
            }
        }

        public async Task<List<Agent>> GetAllAgents(string? q, string? orderBy, bool? onlineOnly, int? page, int? perPage)
        {
            JToken queryResponse = await this.QueryAgents(q, orderBy, onlineOnly, page, perPage).ConfigureAwait(false);

            AgentsObject agentsObject = JsonConvert.DeserializeObject<AgentsObject>(queryResponse.ToString());

            List<Agent> agents = new List<Agent>();

            foreach (Agent agent in agentsObject.Data)
            {
                agents.Add(agent);
            }

            // agents.Count <= agentsObject.TotalCount
            while (page != 648)
            {
                page++;
                queryResponse = await this.QueryAgents(q, orderBy, onlineOnly, page, 100).ConfigureAwait(false);
                agentsObject = JsonConvert.DeserializeObject<AgentsObject>(queryResponse.ToString());
                foreach (Agent agent in agentsObject.Data)
                {
                    agents.Add(agent);
                }
            }

            return agents;
        }

        /// <summary>
        /// Queries an agents profile using the Intruder API
        /// Endpoint(https://api.intruderfps.com/agents/76561198027572754).
        /// </summary>
        /// <param name="steamID">Agent's Steam ID.</param>
        /// <returns>Profile JToken.</returns>
        public async Task<Agent> GetAgentProfileAsync(ulong SteamID)
        {
            StringBuilder urlBuilder3000 = new StringBuilder(BaseUrl);

            urlBuilder3000.Append($"/{SteamID}");

            string rawJson;

            try
            {
                rawJson = await this.httpClient.GetStringAsync(new Uri(urlBuilder3000.ToString())).ConfigureAwait(false);
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, $"Failed to obtain profile data for agent: {SteamID} | {urlBuilder3000.ToString()}");
                return null;
            }

            return JsonConvert.DeserializeObject<Agent>(rawJson);
        }

        /// <summary>
        /// Queries the Intruder API for an agent's stats.
        /// Endpoint(https://api.intruderfps.com/agents/#############/stats).
        /// </summary>
        /// <param name="steamID">Agent's Steam ID.</param>
        /// <returns>AgentStats.</returns>
        public async Task<AgentStats> GetAgentStatsAsync(ulong steamID)
        {
            StringBuilder urlBuilder3000 = new StringBuilder(BaseUrl);
            urlBuilder3000.Append($"/{steamID}{StatsEndpoint}");

            string rawJson;

            try
            {
                rawJson = await this.httpClient.GetStringAsync(new Uri(urlBuilder3000.ToString())).ConfigureAwait(false);
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, $"Failed to run a query for Agents. URL: {urlBuilder3000}");
                return null;
            }

            return JsonConvert.DeserializeObject<AgentStats>(rawJson);
        }

        /// <summary>
        /// Queries an agents votes using the Intruder API
        /// Endpoint(https://api.intruderfps.com/agents/76561198027572754/votes).
        /// </summary>
        /// <param name="steamID">Agent's Steam ID.</param>
        /// <returns>Votes JToken.</returns>
        public async Task<List<AgentVotes>> QueryAgentVotes(ulong steamID)
        {
            StringBuilder urlBuilder3000 = new StringBuilder(BaseUrl);

            urlBuilder3000.Append($"/{steamID}{VotesEndpoint}");

            string rawJson;

            try
            {
                rawJson = await this.httpClient.GetStringAsync(new Uri(urlBuilder3000.ToString())).ConfigureAwait(false);
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, $"Failed to obtain votes for Steam ID: {steamID} | {urlBuilder3000}");
                return null;
            }

            return JsonConvert.DeserializeObject<List<AgentVotes>>(rawJson);
        }

        /// <summary>
        /// Return an agent that is stored in the database.
        /// </summary>
        /// <param name="steamID"></param>
        /// <returns></returns>
        public async Task<AgentsDB> GetDBAgentAsync(ulong steamID)
        {
            AgentsDB agent = new AgentsDB();
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();

            if (await this.CheckDBAgentAsync(steamID).ConfigureAwait(false))
            {
                agent = db.Agents.Where(x => x.SteamID == steamID).FirstOrDefault();
            }
            else
            {
                await this.StoreAgentDBAsync(steamID).ConfigureAwait(false);
            }

            return agent;
        }

        public async Task UpdateDBAgentAsync(Agent agent)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();

            AgentsDB DbAgent = await this.GetDBAgentAsync(agent.SteamID).ConfigureAwait(false);

            if (await this.CheckDBAgentAsync(agent.SteamID).ConfigureAwait(false))
            {
                if (DateTime.Compare(agent.LastLogin, DbAgent.LastLogin) == 1)
                {
                    // Agent has updated stats. lets update our DB.

                    // get stats
                    AgentStats statsResponse = await this.GetAgentStatsAsync(agent.SteamID).ConfigureAwait(false);

                    // get votes
                    List<AgentVotes> votesResponse = await this.QueryAgentVotes(agent.SteamID).ConfigureAwait(false);

                    if (DbAgent.Name != agent.Name)
                    {
                        DbAgent.OldAgentName = DbAgent.Name;
                    }

                    // BEGIN PROFILE INFO
                    DbAgent.SteamAvatar = agent.AvatarURL;
                    DbAgent.Role = agent.Role;
                    DbAgent.Name = agent.Name;
                    DbAgent.LoginCount = agent.LoginCount;
                    DbAgent.LastLogin = agent.LastLogin;
                    DbAgent.LastUpdate = agent.LastUpdate;
                    // END PROFILE INFO
                    // BEGIN STATS INFO
                    DbAgent.MatchesWon = statsResponse.MatchesWon;
                    DbAgent.MatchesLost = statsResponse.MatchesLost;
                    DbAgent.RoundsLost = statsResponse.RoundsLost;
                    DbAgent.RoundsTied = statsResponse.RoundsTied;
                    DbAgent.RoundsWonElim = statsResponse.RoundsWonElim;
                    DbAgent.RoundsWonCapture = statsResponse.RoundsWonCapture;
                    DbAgent.RoundsWonHack = statsResponse.RoundsWonHack;
                    DbAgent.RoundsWonTimer = statsResponse.RoundsWonTimer;
                    DbAgent.RoundsWonCustom = statsResponse.RoundsWonCustom;
                    DbAgent.TimePlayed = statsResponse.TimePlayed;
                    DbAgent.Kills = statsResponse.Kills;
                    DbAgent.TeamKills = statsResponse.TeamKills;
                    DbAgent.Deaths = statsResponse.Deaths;
                    DbAgent.Arrests = statsResponse.Arrests;
                    DbAgent.GotArrested = statsResponse.GotArrested;
                    DbAgent.Captures = statsResponse.Captures;
                    DbAgent.Pickups = statsResponse.Pickups;
                    DbAgent.NetworkHacks = statsResponse.NetworkHacks;
                    DbAgent.Survivals = statsResponse.Survivals;
                    DbAgent.Suicides = statsResponse.Suicides;
                    DbAgent.Knockdowns = statsResponse.Knockdowns;
                    DbAgent.GotKnockedDown = statsResponse.GotKnockedDown;
                    DbAgent.TeamKnockdowns = statsResponse.TeamKnockdowns;
                    DbAgent.TeamDamage = statsResponse.TeamDamage;
                    DbAgent.Level = statsResponse.Level;
                    DbAgent.LevelXP = statsResponse.LevelXP;
                    DbAgent.LevelXPRequired = statsResponse.LevelXPRequired;
                    DbAgent.TotalXP = statsResponse.TotalXP;
                    // END STATS INFO
                    // BEGIN VOTES
                    DbAgent.PositiveVotes = votesResponse.ElementAt(0).PositiveVotes;
                    DbAgent.NegativeVotes = votesResponse.ElementAt(0).NegativeVotes;
                    DbAgent.TotalVotes = votesResponse.ElementAt(0).ReceivedVotes;
                    DbAgent.Timestamp = DateTime.Now;

                    db.Agents.Update(DbAgent);
                }
                else if (DateTime.Compare(agent.LastLogin, DbAgent.LastLogin) == 0)
                {
                    // Same date, do nothing.
                }
            }
            else
            {
                await this.StoreAgentDBAsync(agent.SteamID).ConfigureAwait(false);
            }

            await db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> CheckDBAgentAsync(ulong steamID)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();

            if (db.Agents.Where(x => x.SteamID == steamID).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<AgentsDB> GetDBAgentsAsync(string OrderBy)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();
            List<AgentsDB> agentsDBs = new List<AgentsDB>();
            switch (OrderBy)
            {
                case "match":
                case "matches":
                case "matches won":
                    agentsDBs = db.Agents.OrderByDescending(x => x.MatchesWon).Take(10).ToList();
                    break;

                case "matches lost":
                    agentsDBs = db.Agents.OrderByDescending(x => x.MatchesLost).Take(10).ToList();
                    break;

                case "rounds lost":
                    agentsDBs = db.Agents.OrderByDescending(x => x.RoundsLost).Take(10).ToList();
                    break;

                case "rounds tied":
                    agentsDBs = db.Agents.OrderByDescending(x => x.RoundsTied).Take(10).ToList();
                    break;

                case "kills":
                    agentsDBs = db.Agents.OrderByDescending(x => x.Kills).Take(10).ToList();
                    break;

                case "deaths":
                    agentsDBs = db.Agents.OrderByDescending(x => x.Deaths).Take(10).ToList();
                    break;

                case "arrests":
                    agentsDBs = db.Agents.OrderByDescending(x => x.Arrests).Take(10).ToList();
                    break;

                case "team kills":
                    agentsDBs = db.Agents.OrderByDescending(x => x.TeamKills).Take(10).ToList();
                    break;

                case "captures":
                    agentsDBs = db.Agents.OrderByDescending(x => x.Captures).Take(10).ToList();
                    break;

                case "hacks":
                case "network hacks":
                    agentsDBs = db.Agents.OrderByDescending(x => x.NetworkHacks).Take(10).ToList();
                    break;

                case "survivals":
                    agentsDBs = db.Agents.OrderByDescending(x => x.Survivals).Take(10).ToList();
                    break;

                case "suicides":
                    agentsDBs = db.Agents.OrderByDescending(x => x.Suicides).Take(10).ToList();
                    break;

                case "login count":
                    agentsDBs = db.Agents.OrderByDescending(x => x.LoginCount).Take(10).ToList();
                    break;

                case "pickups":
                    agentsDBs = db.Agents.OrderByDescending(x => x.Pickups).Take(10).ToList();
                    break;

                case "votes":
                    agentsDBs = db.Agents.OrderByDescending(x => x.TotalVotes).Take(10).ToList();
                    break;

                case "xp":
                    agentsDBs = db.Agents.OrderByDescending(x => x.TotalXP).Take(10).ToList();
                    break;

                case "team damage":
                    agentsDBs = db.Agents.OrderByDescending(x => x.TeamDamage).Take(10).ToList();
                    break;

                case "team knockdowns":
                    agentsDBs = db.Agents.OrderByDescending(x => x.TeamKnockdowns).Take(10).ToList();
                    break;

                case "arrested":
                    agentsDBs = db.Agents.OrderByDescending(x => x.GotArrested).Take(10).ToList();
                    break;

                case "got knocked down":
                case "knocked down":
                    agentsDBs = db.Agents.OrderByDescending(x => x.GotArrested).Take(10).ToList();
                    break;

                case "rounds won capture":
                    agentsDBs = db.Agents.OrderByDescending(x => x.RoundsWonCapture).Take(10).ToList();
                    break;

                case "rounds won hack":
                    agentsDBs = db.Agents.OrderByDescending(x => x.RoundsWonHack).Take(10).ToList();
                    break;

                case "rounds won elim":
                    agentsDBs = db.Agents.OrderByDescending(x => x.RoundsWonElim).Take(10).ToList();
                    break;

                case "rounds won timer":
                    agentsDBs = db.Agents.OrderByDescending(x => x.RoundsWonTimer).Take(10).ToList();
                    break;

                case "rounds won custom":
                    agentsDBs = db.Agents.OrderByDescending(x => x.RoundsWonCustom).Take(10).ToList();
                    break;

                case "positive votes":
                    agentsDBs = db.Agents.OrderByDescending(x => x.PositiveVotes).Take(10).ToList();
                    break;

                case "negative votes":
                    agentsDBs = db.Agents.OrderByDescending(x => x.NegativeVotes).Take(10).ToList();
                    break;

                default:
                    agentsDBs = db.Agents.OrderByDescending(x => x.TotalXP).Take(10).ToList();
                    break;
            }
            return agentsDBs;
        }


        /// <summary>
        /// Stores a new Agent DB into the database. Will update if there is an existing entry, but there shouldn't be.
        /// </summary>
        /// <param name="steamID"></param>
        /// <returns></returns>
        public async Task StoreAgentDBAsync(ulong steamID)
        {
            // get profile
            Agent profileResponse = await this.GetAgentProfileAsync(steamID).ConfigureAwait(false);

            // get stats
            AgentStats statsResponse = await this.GetAgentStatsAsync(steamID).ConfigureAwait(false);

            // get votes
            List<AgentVotes> votesResponse = await this.QueryAgentVotes(steamID).ConfigureAwait(false);

            //add to db
            AgentsDB agentsDB = new AgentsDB()
            {
                // BEGIN PROFILE INFO
                SteamID = profileResponse.SteamID,
                SteamAvatar = profileResponse.AvatarURL,
                ID = profileResponse.IntruderID,
                Role = profileResponse.Role,
                Name = profileResponse.Name,
                LoginCount = profileResponse.LoginCount,
                FirstLogin = profileResponse.FirstLogin,
                LastLogin = profileResponse.LastLogin,
                LastUpdate = profileResponse.LastUpdate,
                // END PROFILE INFO
                // BEGIN STATS INFO
                MatchesWon = statsResponse.MatchesWon,
                MatchesLost = statsResponse.MatchesLost,
                RoundsLost = statsResponse.RoundsLost,
                RoundsTied = statsResponse.RoundsTied,
                RoundsWonElim = statsResponse.RoundsWonElim,
                RoundsWonCapture = statsResponse.RoundsWonCapture,
                RoundsWonHack = statsResponse.RoundsWonHack,
                RoundsWonTimer = statsResponse.RoundsWonTimer,
                RoundsWonCustom = statsResponse.RoundsWonCustom,
                TimePlayed = statsResponse.TimePlayed,
                Kills = statsResponse.Kills,
                TeamKills = statsResponse.TeamKills,
                Deaths = statsResponse.Deaths,
                Arrests = statsResponse.Arrests,
                GotArrested = statsResponse.GotArrested,
                Captures = statsResponse.Captures,
                Pickups = statsResponse.Pickups,
                NetworkHacks = statsResponse.NetworkHacks,
                Survivals = statsResponse.Survivals,
                Suicides = statsResponse.Suicides,
                Knockdowns = statsResponse.Knockdowns,
                GotKnockedDown = statsResponse.GotKnockedDown,
                TeamKnockdowns = statsResponse.TeamKnockdowns,
                TeamDamage = statsResponse.TeamDamage,
                Level = statsResponse.Level,
                LevelXP = statsResponse.LevelXP,
                LevelXPRequired = statsResponse.LevelXPRequired,
                TotalXP = statsResponse.TotalXP,
                // END STATS INFO
                // BEGIN VOTES
                PositiveVotes = votesResponse.ElementAt(0).PositiveVotes,
                NegativeVotes = votesResponse.ElementAt(0).NegativeVotes,
                TotalVotes = votesResponse.ElementAt(0).ReceivedVotes,
                Timestamp = DateTime.Now,
            };

            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();

            try
            {
                db.Agents.Add(agentsDB);
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateException e1)
            {
                db.Agents.Update(agentsDB);
                await db.SaveChangesAsync().ConfigureAwait(false);
            }

            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}
