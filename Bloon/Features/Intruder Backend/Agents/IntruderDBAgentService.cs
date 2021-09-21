namespace Bloon.Features.Intruder_Backend.Agents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Bloon.Features.IntruderBackend.Agents;
    using Bloon.Features.PackageAccounts;
    using Microsoft.Extensions.DependencyInjection;
    using Bloon.Features.IntruderBackend.Agents;
    using Bloon.Core.Database;

    public class IntruderDBAgentService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly AccountService accountService;

        public IntruderDBAgentService(IServiceScopeFactory scopeFactory, AccountService accountService)
        {
            this.scopeFactory = scopeFactory;
            this.accountService = accountService;
        }
        /// <summary>
        /// Return an agent that is stored in the database.
        /// </summary>
        /// <param name="steamID">SteamID64.</param>
        /// <returns>An awaitable Task.</returns>
        public async Task<IntruderDBAgent> GetDBAgentAsync(string usernameOrSteamID)
        {
            List<IntruderDBAgent> agentsDBs = new List<IntruderDBAgent>();
            IntruderDBAgent agent = new IntruderDBAgent();
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();

            agentsDBs = db.OldAgents.ToList();
            agent = agentsDBs.Where(x => x.Name.Contains(usernameOrSteamID)).FirstOrDefault();
            return agent;
        }

        private bool CheckDBAgent(ulong steamID)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();

            return db.Agents.Where(x => x.SteamID == steamID).Any();
        }
    }
}
