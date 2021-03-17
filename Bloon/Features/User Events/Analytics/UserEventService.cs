namespace Bloon.Features.Analytics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bloon.Analytics.Users;
    using Bloon.Core.Database;
    using DSharpPlus.EventArgs;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    public class UserEventService
    {
        private readonly IServiceScopeFactory scopeFactory;

        public UserEventService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public async Task AddUserJoinedEventAsync(GuildMemberAddEventArgs args)
        {
            UserEvent userEvent = new ()
            {
                UserId = args.Member.Id,
                Username = args.Member.Username,
                Nickname = args.Member.Nickname,
                Discriminator = args.Member.Discriminator,
                AvatarId = args.Member.AvatarHash.ToString(),
                AccountCreated = args.Member.CreationTimestamp.UtcDateTime,
                Bot = args.Member.IsBot,
                Event = Event.Joined,
                Timestamp = DateTime.Now,
            };

            try
            {
                using IServiceScope scope = this.scopeFactory.CreateScope();
                using AnalyticsContext db = scope.ServiceProvider.GetRequiredService<AnalyticsContext>();
                db.UserEvents.Add(userEvent);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error(e, $"Failed to add new user event {userEvent.Username}#{userEvent.Discriminator} | {userEvent.UserId}");
                return;
            }
        }

        public async Task AddUserLeftEventAsync(GuildMemberRemoveEventArgs args)
        {
            UserEvent userEvent = new ()
            {
                UserId = args.Member.Id,
                Username = args.Member.Username,
                Nickname = args.Member.Nickname,
                Discriminator = args.Member.Discriminator,
                AvatarId = args.Member.AvatarHash.ToString(),
                AccountCreated = args.Member.CreationTimestamp.UtcDateTime,
                Bot = args.Member.IsBot,
                Event = Event.Left,
                Timestamp = DateTime.Now,
            };

            try
            {
                using IServiceScope scope = this.scopeFactory.CreateScope();
                using AnalyticsContext db = scope.ServiceProvider.GetRequiredService<AnalyticsContext>();
                db.UserEvents.Add(userEvent);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error(e, $"Failed to add new user event {userEvent.Username}#{userEvent.Discriminator} | {userEvent.UserId}");
                return;
            }
        }

        public async Task AddUserBannedEventAsync(GuildBanAddEventArgs args)
        {
            UserEvent userEvent = new ()
            {
                UserId = args.Member.Id,
                Username = args.Member.Username,
                Nickname = args.Member.Nickname,
                Discriminator = args.Member.Discriminator,
                AvatarId = args.Member.AvatarHash.ToString(),
                AccountCreated = args.Member.CreationTimestamp.UtcDateTime,
                Bot = args.Member.IsBot,
                Event = Event.Banned,
                Timestamp = DateTime.Now,
            };

            try
            {
                using IServiceScope scope = this.scopeFactory.CreateScope();
                using AnalyticsContext db = scope.ServiceProvider.GetRequiredService<AnalyticsContext>();
                db.UserEvents.Add(userEvent);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error(e, $"Failed to add new user event {userEvent.Username}#{userEvent.Discriminator} | {userEvent.UserId}");
                return;
            }
        }

        public async Task AddUserUnBannedEventAsync(GuildBanRemoveEventArgs args)
        {
            UserEvent userEvent = new ()
            {
                UserId = args.Member.Id,
                Username = args.Member.Username,
                Nickname = args.Member.Nickname,
                Discriminator = args.Member.Discriminator,
                AvatarId = args.Member.AvatarHash.ToString(),
                AccountCreated = args.Member.CreationTimestamp.UtcDateTime,
                Bot = args.Member.IsBot,
                Event = Event.Unbanned,
                Timestamp = DateTime.Now,
            };

            try
            {
                using IServiceScope scope = this.scopeFactory.CreateScope();
                using AnalyticsContext db = scope.ServiceProvider.GetRequiredService<AnalyticsContext>();
                db.UserEvents.Add(userEvent);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error(e, $"Failed to add new user event {userEvent.Username}#{userEvent.Discriminator} | {userEvent.UserId}");
                return;
            }
        }

        public List<UserEvent> QueryEventReportAsync(ulong discordId)
        {
            List<UserEvent> dbUserEvents = new ();

            using IServiceScope scope = this.scopeFactory.CreateScope();
            using AnalyticsContext db = scope.ServiceProvider.GetRequiredService<AnalyticsContext>();
            try
            {
                var dbQuery = db.UserEvents;
                foreach (UserEvent events in dbQuery.Where(x => x.UserId == discordId))
                {
                    dbUserEvents.Add(events);
                }

                return dbUserEvents;
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to query all user events!");
            }

            return dbUserEvents;
        }
    }
}
