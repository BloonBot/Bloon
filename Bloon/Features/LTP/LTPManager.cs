namespace Bloon.Features.LTP
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Core.Commands.Attributes;
    using Bloon.Core.Database;
    using Bloon.Variables.Roles;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;
    using DSharpPlus.Entities;

    public class LTPManager
    {
        private readonly BloonContext db;

        public LTPManager(BloonContext db)
        {
            this.db = db;
        }

        public async void AddLookingToPlayUser(ulong userID)
        {
            this.db.LTPJoins.Add(new LTPJoin()
            {
                UserId = userID,
                Timestamp = DateTime.UtcNow,
            });
            await this.db.SaveChangesAsync();
        }

        public async void RemoveLookingToPlayUser(ulong userID)
        {
            this.db.Remove(new LTPJoin()
            {
                UserId = userID,
            });
            await this.db.SaveChangesAsync();
        }
    }
}
