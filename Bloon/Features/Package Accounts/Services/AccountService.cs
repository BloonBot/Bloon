namespace Bloon.Features.PackageAccounts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using DSharpPlus.Entities;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    public class AccountService
    {
        private readonly IServiceScopeFactory scopeFactory;

        public AccountService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public async Task AddAsync(DiscordUser user, DateTime accountCreated)
        {
            // Before adding the user into the database, we want to give that user some base properties.
            PackageAccount account = new PackageAccount()
            {
                DiscordID = user.Id,
                LastLogin = DateTime.Now,
                AccountCreated = accountCreated.ToUniversalTime(),
                Type = Permission.Basic,
            };

            try
            {
                using IServiceScope scope = this.scopeFactory.CreateScope();
                using AccountsContext db = scope.ServiceProvider.GetRequiredService<AccountsContext>();
                db.Accounts.Add(account);
                await db.SaveChangesAsync();

                Log.Debug($"Added new account to Database: {user.Username}#{user.Discriminator} | {user.Id}");
            }
            catch (Exception e)
            {
                // Catch if this doesn't happen which should never happen but thats not very good reason to not include this.
                Log.Error(e, $"Couldn't add new account! {user.Username}#{user.Discriminator} | {user.Id}");
                return;
            }
        }

        /// <summary>
        /// Returns a list of package accounts.
        /// </summary>
        /// <returns>All Package Accounts.</returns>
        public List<PackageAccount> QueryAccounts()
        {
            List<PackageAccount> dbAccounts = new List<PackageAccount>();

            using IServiceScope scope = this.scopeFactory.CreateScope();
            using AccountsContext db = scope.ServiceProvider.GetRequiredService<AccountsContext>();
            try
            {
                var dbQuery = db.Accounts;
                foreach (PackageAccount account in dbQuery)
                {
                    dbAccounts.Add(account);
                }

                return dbAccounts;
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to query all package accounts!");
            }

            return dbAccounts;
        }

        public PackageAccount FindAccount(ulong id)
        {
            using IServiceScope scope = this.scopeFactory.CreateScope();
            using AccountsContext db = scope.ServiceProvider.GetRequiredService<AccountsContext>();
            return db.Accounts.Where(x => x.DiscordID == id).FirstOrDefault();
        }
    }
}
