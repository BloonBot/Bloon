namespace Bloon.Features.IntruderBackend.Servers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Features.Censor;
    using Bloon.Features.IntruderBackend.Rooms;
    using Bloon.Utils;
    using Bloon.Variables.Emojis;
    using IntruderLib;
    using IntruderLib.Models;
    using IntruderLib.Models.Rooms;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

    public class RoomService
    {
        private readonly IntruderAPI intruderAPI;
        private readonly IServiceScopeFactory scopeFactory;

        public RoomService(IntruderAPI intruderAPI, IServiceScopeFactory scopeFactory)
        {
            this.intruderAPI = intruderAPI;
            this.scopeFactory = scopeFactory;
        }

        // https://stackoverflow.com/questions/592248/how-can-i-check-if-the-current-time-is-between-in-a-time-frame
        // ty u bueaitiful sonnabitch
        //          ^ Noice
        public static bool IsTimeOfDayBetween(DateTime time, TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime == startTime)
            {
                return true;
            }
            else if (endTime < startTime)
            {
                return time.TimeOfDay <= endTime ||
                    time.TimeOfDay >= startTime;
            }
            else
            {
                return time.TimeOfDay >= startTime &&
                    time.TimeOfDay <= endTime;
            }
        }

        public async Task<List<Room>> GetRoomsAsync(RoomListFilter filter)
        {
            PaginatedResult<Room> results = await this.intruderAPI.GetRoomsAsync(filter);

            return results.Data;
        }

        public async Task<CurrentServerInfo> GetCSIRooms(RoomListFilter filter)
        {
            filter.OrderBy = "official:desc";
            filter.PerPage = 100;

            PaginatedResult<Room> results = await this.intruderAPI.GetRoomsAsync(filter);
            List<RoomDB> rooms = new ();

            int totalPlayers = 0;

            foreach (RoomDB room in results.Data)
            {
                if (room.Official)
                {
                    room.ServerIcon = $"<:os:{ServerEmojis.Official}>";
                }
                else
                {
                    room.ServerIcon = $"<:uo:{ServerEmojis.Unofficial}>";
                }

                if (room.Passworded)
                {
                    room.ServerIcon = $"<:asd:{ServerEmojis.Password}>";
                }

                room.Name = FilterRoomNames(room.Name);
                rooms.Add(room);
                room.RegionFlag = ConvertRegion(room.Region);
                totalPlayers += room.AgentCount;
            }

            // MAKE SURE THE ROOM COUNT IN OUR MODEL MATCHES THE ROOMOBJECT REQUEST
            while (rooms.Count != results.TotalCount)
            {
                filter.Page++;
                results = await this.intruderAPI.GetRoomsAsync(filter);

                foreach (RoomDB room in results.Data)
                {
                    if (room.Official)
                    {
                        room.ServerIcon = $"<:os:{ServerEmojis.Official}>";
                    }
                    else
                    {
                        room.ServerIcon = $"<:uo:{ServerEmojis.Unofficial}>";
                    }

                    if (room.Passworded)
                    {
                        room.ServerIcon = $"<:asd:{ServerEmojis.Password}>";
                    }

                    rooms.Add(room);
                    room.RegionFlag = ConvertRegion(room.Region);
                    totalPlayers += room.AgentCount;
                }
            }

            CurrentServerInfo csi = CountRegions(rooms);
            csi.PlayerCount = totalPlayers;

            return csi;
        }

        public async Task ArchiveRoomData(List<Room> rooms)
        {
            if (rooms.Count != 0)
            {
                using IServiceScope scope = this.scopeFactory.CreateScope();
                using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();

                foreach (RoomDB room in rooms)
                {
                    try
                    {
                        room.DBCreatorSteamId = room.Creator.SteamId;
                        room.DBCurrentMap = (long)room.CurrentMap.Id;
                        if (room.Password != null)
                        {
                            room.Passworded = true;
                        }
                        else
                        {
                            room.Passworded = false;
                        }

                        RoomDB dbRoom = db.Rooms.Where(x => x.RoomId == room.RoomId).FirstOrDefault();

                        if (dbRoom == null)
                        {
                            db.Rooms.Add(room);
                        }
                        else
                        {
                            dbRoom.Name = room.Name;
                            dbRoom.Passworded = room.Passworded;
                            dbRoom.Region = room.Region;
                            dbRoom.Official = room.Official;
                            dbRoom.Ranked = room.Ranked;
                            dbRoom.Version = room.Version;
                            dbRoom.Position = room.Position;
                            dbRoom.AgentCount = room.AgentCount;
                            dbRoom.CreatorId = room.CreatorId;
                            dbRoom.DBCreatorSteamId = room.DBCreatorSteamId;
                            dbRoom.LastUpdate = room.LastUpdate;
                            dbRoom.DBCurrentMap = (long)room.CurrentMap.Id;
                            db.Update(dbRoom);
                        }

                        await db.SaveChangesAsync();
                    }
                    catch (Exception e1)
                    {
                        Log.Error(e1, $"ROOM ID: {room.RoomId} | Name: {room.Name} | Creator: {room.Creator.Name}");
                    }
                }
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        private static string ConvertRegion(string region)
        {
            string flag = string.Empty;

            switch (region)
            {
                case "EU":
                    flag = RegionFlagEmojis.EU;
                    break;
                case "US":
                    flag = RegionFlagEmojis.US;
                    break;
                case "Asia":
                    flag = RegionFlagEmojis.Asia;
                    break;
                case "JP":
                    flag = RegionFlagEmojis.JP;
                    break;
                case "AU":
                    flag = RegionFlagEmojis.AU;
                    break;
                case "USW":
                    flag = RegionFlagEmojis.US;
                    break;
                case "SA":
                    flag = RegionFlagEmojis.SA;
                    break;
                case "CAE":
                    flag = RegionFlagEmojis.CAE;
                    break;
                case "KR":
                    flag = RegionFlagEmojis.KR;
                    break;
                case "IN":
                    flag = RegionFlagEmojis.IN;
                    break;
                case "RU":
                    flag = RegionFlagEmojis.RU;
                    break;
            }

            return flag;
        }

        private static string FilterRoomNames(string roomName)
        {
            // Remove [SeriousPlay] tag in room name
            roomName = roomName.Replace("[SeriousPlay]", string.Empty);
            roomName = roomName.Replace("[Casual]", string.Empty);
            Censor censor = new (File.ReadAllLines(Directory.GetCurrentDirectory() + "/Features/Censor/naughtywords.txt", Encoding.UTF8));
            if (censor.HasNaughtyWord(roomName))
            {
                roomName = "Loser's Room";
            }

            roomName = roomName.Truncate(20);
            return roomName;
        }

        private static ulong CheckTimeOfDay(DateTime time)
        {
            // time is less between 12:AM & 3:59AM
            if (IsTimeOfDayBetween(time, new TimeSpan(0, 0, 0), new TimeSpan(3, 59, 0)))
            {
                return DayNightEmojis.Moon;
            }

            // time is less between 04:00AM & 6:59AM
            if (IsTimeOfDayBetween(time, new TimeSpan(4, 0, 0), new TimeSpan(6, 59, 0)))
            {
                return DayNightEmojis.Moonset;
            }

            // time is less between 7:00AM & 11:59AM
            if (IsTimeOfDayBetween(time, new TimeSpan(7, 0, 0), new TimeSpan(11, 59, 0)))
            {
                return DayNightEmojis.Sunrise;
            }

            // time is less between 12:00PM & 4:59PM
            if (IsTimeOfDayBetween(time, new TimeSpan(12, 0, 0), new TimeSpan(16, 59, 0)))
            {
                return DayNightEmojis.Sun;
            }

            // time is less between 5:00PM & 7:59PM
            if (IsTimeOfDayBetween(time, new TimeSpan(17, 0, 0), new TimeSpan(19, 59, 0)))
            {
                return DayNightEmojis.Sunset;
            }

            // time is less between 8:00PM & 10:59PM
            if (IsTimeOfDayBetween(time, new TimeSpan(20, 0, 0), new TimeSpan(22, 59, 0)))
            {
                return DayNightEmojis.Moonrise;
            }

            // time is less between 11:00PM & 12:59PM
            if (IsTimeOfDayBetween(time, new TimeSpan(23, 0, 0), new TimeSpan(24, 59, 0)))
            {
                return DayNightEmojis.Moon;
            }

            return DayNightEmojis.Sunrise;
        }

        private static CurrentServerInfo CountRegions(List<RoomDB> rooms)
        {
            CurrentServerInfo csi = new ()
            {
                Rooms = rooms,
            };

            foreach (RoomDB regionRooms in rooms)
            {
                switch (regionRooms.Region)
                {
                    case "EU":
                        csi.EUPlayerCount += regionRooms.AgentCount;
                        csi.EURoomCount++;
                        break;
                    case "US":
                        csi.USPlayerCount += regionRooms.AgentCount;
                        csi.USRoomCount++;
                        break;
                    case "AU":
                        csi.AUPlayerCount += regionRooms.AgentCount;
                        csi.AURoomCount++;
                        break;
                    case "USW":
                        csi.USPlayerCount += regionRooms.AgentCount;
                        csi.USRoomCount++;
                        break;
                    case "RU":
                        csi.RUPlayerCount += regionRooms.AgentCount;
                        csi.RURoomCount++;
                        break;
                    case "Asia":
                        csi.AsiaPlayerCount += regionRooms.AgentCount;
                        csi.AsiaRoomCount++;
                        break;
                    case "JP":
                        csi.JPPlayerCount += regionRooms.AgentCount;
                        csi.JPRoomCount++;
                        break;
                    case "SA":
                        csi.SAPlayerCount += regionRooms.AgentCount;
                        csi.SARoomCount++;
                        break;
                    case "CAE":
                        csi.CAEPlayerCount += regionRooms.AgentCount;
                        csi.CAERoomCount++;
                        break;
                    case "KR":
                        csi.KRPlayerCount += regionRooms.AgentCount;
                        csi.KRRoomCount++;
                        break;
                    case "IN":
                        csi.INPlayerCount += regionRooms.AgentCount;
                        csi.INRoomCount++;
                        break;
                }
            }

            // US WEST csi.USWTOD = this.CheckTimeOfDay(DateTime.UtcNow.AddHours(-8.0));
            csi.CAETOD = CheckTimeOfDay(DateTime.UtcNow.AddHours(-5.0));
            csi.USTOD = CheckTimeOfDay(DateTime.UtcNow.AddHours(-5.0));
            csi.SATOD = CheckTimeOfDay(DateTime.UtcNow.AddHours(-3.0));
            csi.EUTOD = CheckTimeOfDay(DateTime.UtcNow.AddHours(1.0));
            csi.RUTOD = CheckTimeOfDay(DateTime.UtcNow.AddHours(3.0));
            csi.INTOD = CheckTimeOfDay(DateTime.UtcNow.AddHours(5.5));
            csi.ASTOD = CheckTimeOfDay(DateTime.UtcNow.AddHours(8.0));
            csi.JPTOD = CheckTimeOfDay(DateTime.UtcNow.AddHours(9.0));
            csi.KRTOD = CheckTimeOfDay(DateTime.UtcNow.AddHours(9.0));
            csi.AUTOD = CheckTimeOfDay(DateTime.UtcNow.AddHours(11.0));

            return csi;
        }
    }
}
