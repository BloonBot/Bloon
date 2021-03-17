namespace Bloon.Features.IntruderBackend.Agents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Features.PackageAccounts;
    using IntruderLib;
    using IntruderLib.Models;
    using IntruderLib.Models.Agents;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    public class AgentService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IntruderAPI intruderAPI;
        private readonly AccountService accountService;

        public AgentService(IServiceScopeFactory scopeFactory, IntruderAPI intruderAPI, AccountService accountService)
        {
            this.scopeFactory = scopeFactory;
            this.intruderAPI = intruderAPI;
            this.accountService = accountService;
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
            List<Agent> agents = new ();
            foreach (PackageAccount account in packageAccounts)
            {
                agents.Add(await this.GetAgentProfileAsync(account.SteamID));
            }

            foreach (Agent dbAgent in agents)
            {
                // Collect Agent Stats
                Stats statsResponse = await this.GetAgentStatsAsync(dbAgent.SteamId);

                // Collect Agent Votes
                List<VoteSummary> votesResponse = await this.QueryAgentVotes(dbAgent.SteamId);

                // Collect Profile Information
                Agent agentResponse = await this.GetAgentProfileAsync(dbAgent.SteamId);

                // If the player's latest lastUpdate value is greater than what we have stored in the database
                if (statsResponse.LastUpdate > dbAgent.LastUpdate && agentResponse.LoginCount > dbAgent.LoginCount)
                {
                    AgentHistory agentHistory = new ()
                    {
                        SteamID = dbAgent.SteamId,
                        MatchesWon = statsResponse.MatchesWon,
                        MatchesLost = statsResponse.MatchesLost,
                        RoundsLost = statsResponse.RoundsLost,
                        RoundsTied = statsResponse.RoundsTied,
                        RoundsWonElim = statsResponse.RoundsWonElimination,
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
                        LevelXP = statsResponse.LevelXp,
                        TotalXP = statsResponse.TotalXp,
                        Level = statsResponse.Level,
                        PositiveVotes = votesResponse.FirstOrDefault().Positive,
                        NegativeVotes = votesResponse.FirstOrDefault().Negative,
                        TotalVotes = votesResponse.FirstOrDefault().Received,
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
                        await db.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, $"Failed to store agent histoy in database. Agent: {agentHistory.ID}");
                    }

                    // Update Agent to include most recent XP.
                    // dbAgent.LastUpdate = DateTime.Now;
                    // dbAgent.XP = statsResponse.TotalXP;
                    // try
                    // {
                    //    db.Agents.Update(dbAgent);
                    //    await db.SaveChangesAsync();
                    // }
                    // catch (Exception e)
                    // {
                    //    Log.Error(e, $"Failed to update agent details in database. Agent: {dbAgent.Name} | ID: {dbAgent.SteamID}");
                    // }
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
            List<LeveledAgent> agents = await this.GetAllAgents(new AgentListFilter
            {
                Page = 101,
                PerPage = 100,
            });

            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();

            foreach (Agent agent in agents)
            {
                if (db.Agents.Where(x => x.SteamID == agent.SteamId).AsNoTracking().FirstOrDefault() == null || db.Agents.Any(x => x.SteamID != agent.SteamId) || db.Agents.Any(x => x.ID != agent.Id))
                {
                    // Collect Agent Stats
                    Stats statsResponse = await this.GetAgentStatsAsync(agent.SteamId);

                    // Collect Agent Votes
                    List<VoteSummary> votesResponse = await this.QueryAgentVotes(agent.SteamId);

                    AgentsDB agentsDB = new ()
                    {
                        // BEGIN PROFILE INFO
                        SteamID = agent.SteamId,
                        SteamAvatar = agent.AvatarUrl,
                        ID = agent.Id,
                        Role = agent.Role,
                        Name = agent.Name,
                        LoginCount = agent.LoginCount,
                        FirstLogin = agent.FirstLogin,
                        LastLogin = agent.LastLogin,
                        LastUpdate = agent.LastUpdate,

                        // BEGIN STATS INFO
                        MatchesWon = statsResponse.MatchesWon,
                        MatchesLost = statsResponse.MatchesLost,
                        RoundsLost = statsResponse.RoundsLost,
                        RoundsTied = statsResponse.RoundsTied,
                        RoundsWonElim = statsResponse.RoundsWonElimination,
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
                        LevelXP = statsResponse.LevelXp,
                        LevelXPRequired = statsResponse.LevelXpRequired,
                        TotalXP = statsResponse.TotalXp,

                        // BEGIN VOTES
                        PositiveVotes = votesResponse.ElementAt(0).Positive,
                        NegativeVotes = votesResponse.ElementAt(0).Negative,
                        TotalVotes = votesResponse.ElementAt(0).Received,
                        Timestamp = DateTime.Now,
                    };
                    try
                    {
                        db.Agents.Add(agentsDB);
                        await db.SaveChangesAsync();
                    }
                    catch (DbUpdateException)
                    {
                        db.Agents.Update(agentsDB);
                        await db.SaveChangesAsync();
                    }
                }
                else
                {
                    Console.WriteLine($"{agent.Name} IS ALREADY STORED. INTRUDER ID: {agent.Id}");
                }
            }
        }

        public async Task<List<LeveledAgent>> GetAllAgents(AgentListFilter filter)
        {
            PaginatedResult<LeveledAgent> results = await this.intruderAPI.GetAgentsAsync(filter);
            List<LeveledAgent> agents = new ();
            agents.AddRange(results.Data);

            // agents.Count <= agentsObject.TotalCount
            while (results.Page != 648)
            {
                filter.Page++;
                results = await this.intruderAPI.GetAgentsAsync(filter);
                agents.AddRange(results.Data);
            }

            return agents;
        }

        /// <summary>
        /// Queries an agents profile using the Intruder API
        /// Endpoint(https://api.intruderfps.com/agents/76561198027572754).
        /// </summary>
        /// <param name="steamID">Agent's Steam ID.</param>
        /// <returns>Profile JToken.</returns>
        public async Task<LeveledAgent> GetAgentProfileAsync(ulong steamID)
        {
            LeveledAgent agent = null;

            try
            {
                agent = await this.intruderAPI.GetAgentAsync(steamID);
            }
            catch (APIException e)
            {
                Log.Error(e, $"Failed to obtain profile data for agent: {steamID}");
            }

            return agent;
        }

        /// <summary>
        /// Queries the Intruder API for an agent's stats.
        /// Endpoint(https://api.intruderfps.com/agents/#############/stats).
        /// </summary>
        /// <param name="steamID">Agent's Steam ID.</param>
        /// <returns>AgentStats.</returns>
        public async Task<Stats> GetAgentStatsAsync(ulong steamID)
        {
            Stats stats = null;

            try
            {
                stats = await this.intruderAPI.GetAgentStatsAsync(steamID);
            }
            catch (APIException e)
            {
                Log.Error(e, $"Failed to fetch stats for: {steamID}");
            }

            return stats;
        }

        /// <summary>
        /// Queries an agents votes using the Intruder API
        /// Endpoint(https://api.intruderfps.com/agents/76561198027572754/votes).
        /// </summary>
        /// <param name="steamID">Agent's Steam ID.</param>
        /// <returns>Votes JToken.</returns>
        public async Task<List<VoteSummary>> QueryAgentVotes(ulong steamID)
        {
            List<VoteSummary> votes = null;

            try
            {
                votes = await this.intruderAPI.GetAgentVotesAsync(steamID);
            }
            catch (APIException e)
            {
                Log.Error(e, $"Failed to obtain votes for Steam ID: {steamID}");
            }

            return votes;
        }

        /// <summary>
        /// Return an agent that is stored in the database.
        /// </summary>
        /// <param name="steamID">SteamID64.</param>
        /// <returns>An awaitable Task.</returns>
        public async Task<AgentsDB> GetDBAgentAsync(ulong steamID)
        {
            AgentsDB agent = new ();
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();

            if (this.CheckDBAgent(steamID))
            {
                agent = db.Agents.Where(x => x.SteamID == steamID).FirstOrDefault();
            }
            else
            {
                await this.StoreAgentDBAsync(steamID);
            }

            return agent;
        }

        public async Task UpdateDBAgentAsync(Agent agent)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();

            AgentsDB dbAgent = await this.GetDBAgentAsync(agent.SteamId);

            if (this.CheckDBAgent(agent.SteamId))
            {
                if (DateTime.Compare(agent.LastLogin, dbAgent.LastLogin) == 1)
                {
                    // Agent has updated stats. lets update our DB.

                    // get stats
                    Stats statsResponse = await this.GetAgentStatsAsync(agent.SteamId);

                    // get votes
                    List<VoteSummary> votesResponse = await this.QueryAgentVotes(agent.SteamId);

                    if (dbAgent.Name != agent.Name)
                    {
                        dbAgent.OldAgentName = dbAgent.Name;
                    }

                    // BEGIN PROFILE INFO
                    dbAgent.SteamAvatar = agent.AvatarUrl;
                    dbAgent.Role = agent.Role;
                    dbAgent.Name = agent.Name;
                    dbAgent.LoginCount = agent.LoginCount;
                    dbAgent.LastLogin = agent.LastLogin;
                    dbAgent.LastUpdate = agent.LastUpdate;

                    // BEGIN STATS INFO
                    dbAgent.MatchesWon = statsResponse.MatchesWon;
                    dbAgent.MatchesLost = statsResponse.MatchesLost;
                    dbAgent.RoundsLost = statsResponse.RoundsLost;
                    dbAgent.RoundsTied = statsResponse.RoundsTied;
                    dbAgent.RoundsWonElim = statsResponse.RoundsWonElimination;
                    dbAgent.RoundsWonCapture = statsResponse.RoundsWonCapture;
                    dbAgent.RoundsWonHack = statsResponse.RoundsWonHack;
                    dbAgent.RoundsWonTimer = statsResponse.RoundsWonTimer;
                    dbAgent.RoundsWonCustom = statsResponse.RoundsWonCustom;
                    dbAgent.TimePlayed = statsResponse.TimePlayed;
                    dbAgent.Kills = statsResponse.Kills;
                    dbAgent.TeamKills = statsResponse.TeamKills;
                    dbAgent.Deaths = statsResponse.Deaths;
                    dbAgent.Arrests = statsResponse.Arrests;
                    dbAgent.GotArrested = statsResponse.GotArrested;
                    dbAgent.Captures = statsResponse.Captures;
                    dbAgent.Pickups = statsResponse.Pickups;
                    dbAgent.NetworkHacks = statsResponse.NetworkHacks;
                    dbAgent.Survivals = statsResponse.Survivals;
                    dbAgent.Suicides = statsResponse.Suicides;
                    dbAgent.Knockdowns = statsResponse.Knockdowns;
                    dbAgent.GotKnockedDown = statsResponse.GotKnockedDown;
                    dbAgent.TeamKnockdowns = statsResponse.TeamKnockdowns;
                    dbAgent.TeamDamage = statsResponse.TeamDamage;
                    dbAgent.Level = statsResponse.Level;
                    dbAgent.LevelXP = statsResponse.LevelXp;
                    dbAgent.LevelXPRequired = statsResponse.LevelXpRequired;
                    dbAgent.TotalXP = statsResponse.TotalXp;

                    // BEGIN VOTES
                    dbAgent.PositiveVotes = votesResponse.ElementAt(0).Positive;
                    dbAgent.NegativeVotes = votesResponse.ElementAt(0).Negative;
                    dbAgent.TotalVotes = votesResponse.ElementAt(0).Received;
                    dbAgent.Timestamp = DateTime.Now;

                    db.Agents.Update(dbAgent);
                }
                else if (DateTime.Compare(agent.LastLogin, dbAgent.LastLogin) == 0)
                {
                    // Same date, do nothing.
                }
            }
            else
            {
                await this.StoreAgentDBAsync(agent.SteamId);
            }

            await db.SaveChangesAsync();
        }

        public bool CheckDBAgent(ulong steamID)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();

            return db.Agents.Where(x => x.SteamID == steamID).Any();
        }

        public List<AgentsDB> GetDBAgentsAsync(string orderBy)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();
            List<AgentsDB> agentsDBs = new ();
            agentsDBs = orderBy switch
            {
                "match" or "matches" or "matches won" => db.Agents.OrderByDescending(x => x.MatchesWon).Take(10).ToList(),
                "matches lost" => db.Agents.OrderByDescending(x => x.MatchesLost).Take(10).ToList(),
                "rounds lost" => db.Agents.OrderByDescending(x => x.RoundsLost).Take(10).ToList(),
                "rounds tied" => db.Agents.OrderByDescending(x => x.RoundsTied).Take(10).ToList(),
                "kills" => db.Agents.OrderByDescending(x => x.Kills).Take(10).ToList(),
                "deaths" => db.Agents.OrderByDescending(x => x.Deaths).Take(10).ToList(),
                "arrests" => db.Agents.OrderByDescending(x => x.Arrests).Take(10).ToList(),
                "team kills" => db.Agents.OrderByDescending(x => x.TeamKills).Take(10).ToList(),
                "captures" => db.Agents.OrderByDescending(x => x.Captures).Take(10).ToList(),
                "hacks" or "network hacks" => db.Agents.OrderByDescending(x => x.NetworkHacks).Take(10).ToList(),
                "survivals" => db.Agents.OrderByDescending(x => x.Survivals).Take(10).ToList(),
                "suicides" => db.Agents.OrderByDescending(x => x.Suicides).Take(10).ToList(),
                "login count" => db.Agents.OrderByDescending(x => x.LoginCount).Take(10).ToList(),
                "pickups" => db.Agents.OrderByDescending(x => x.Pickups).Take(10).ToList(),
                "votes" => db.Agents.OrderByDescending(x => x.TotalVotes).Take(10).ToList(),
                "xp" => db.Agents.OrderByDescending(x => x.TotalXP).Take(10).ToList(),
                "team damage" => db.Agents.OrderByDescending(x => x.TeamDamage).Take(10).ToList(),
                "team knockdowns" => db.Agents.OrderByDescending(x => x.TeamKnockdowns).Take(10).ToList(),
                "arrested" => db.Agents.OrderByDescending(x => x.GotArrested).Take(10).ToList(),
                "got knocked down" or "knocked down" => db.Agents.OrderByDescending(x => x.GotArrested).Take(10).ToList(),
                "rounds won capture" => db.Agents.OrderByDescending(x => x.RoundsWonCapture).Take(10).ToList(),
                "rounds won hack" => db.Agents.OrderByDescending(x => x.RoundsWonHack).Take(10).ToList(),
                "rounds won elim" => db.Agents.OrderByDescending(x => x.RoundsWonElim).Take(10).ToList(),
                "rounds won timer" => db.Agents.OrderByDescending(x => x.RoundsWonTimer).Take(10).ToList(),
                "rounds won custom" => db.Agents.OrderByDescending(x => x.RoundsWonCustom).Take(10).ToList(),
                "positive votes" => db.Agents.OrderByDescending(x => x.PositiveVotes).Take(10).ToList(),
                "negative votes" => db.Agents.OrderByDescending(x => x.NegativeVotes).Take(10).ToList(),
                _ => db.Agents.OrderByDescending(x => x.TotalXP).Take(10).ToList(),
            };
            return agentsDBs;
        }

        /// <summary>
        /// Stores a new Agent DB into the database. Will update if there is an existing entry, but there shouldn't be.
        /// </summary>
        /// <param name="steamID">SteamID64.</param>
        /// <returns>An awaitable task.</returns>
        public async Task StoreAgentDBAsync(ulong steamID)
        {
            // get profile
            Agent profileResponse = await this.GetAgentProfileAsync(steamID);

            // get stats
            Stats statsResponse = await this.GetAgentStatsAsync(steamID);

            // get votes
            List<VoteSummary> votesResponse = await this.QueryAgentVotes(steamID);

            // add to db
            AgentsDB agentsDB = new ()
            {
                // BEGIN PROFILE INFO
                SteamID = profileResponse.SteamId,
                SteamAvatar = profileResponse.AvatarUrl,
                ID = profileResponse.Id,
                Role = profileResponse.Role,
                Name = profileResponse.Name,
                LoginCount = profileResponse.LoginCount,
                FirstLogin = profileResponse.FirstLogin,
                LastLogin = profileResponse.LastLogin,
                LastUpdate = profileResponse.LastUpdate,

                // BEGIN STATS INFO
                MatchesWon = statsResponse.MatchesWon,
                MatchesLost = statsResponse.MatchesLost,
                RoundsLost = statsResponse.RoundsLost,
                RoundsTied = statsResponse.RoundsTied,
                RoundsWonElim = statsResponse.RoundsWonElimination,
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
                LevelXP = statsResponse.LevelXp,
                LevelXPRequired = statsResponse.LevelXpRequired,
                TotalXP = statsResponse.TotalXp,

                // BEGIN VOTES
                PositiveVotes = votesResponse.ElementAt(0).Positive,
                NegativeVotes = votesResponse.ElementAt(0).Negative,
                TotalVotes = votesResponse.ElementAt(0).Received,
                Timestamp = DateTime.Now,
            };

            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();

            try
            {
                db.Agents.Add(agentsDB);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                db.Agents.Update(agentsDB);
                await db.SaveChangesAsync();
            }

            await Task.CompletedTask;
        }
    }
}
