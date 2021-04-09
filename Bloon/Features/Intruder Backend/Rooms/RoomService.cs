namespace Bloon.Features.IntruderBackend.Servers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Bloon.Core.Database;
    using Bloon.Features.Censor;
    using Bloon.Features.IntruderBackend.Rooms;
    using Bloon.Utils;
    using Bloon.Variables.Emojis;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Serilog;

    public class RoomService
    {
        private const string BaseUrl = "https://api.intruderfps.com/rooms?HideEmpty=true";

        private readonly HttpClient httpClient;
        private readonly IServiceScopeFactory scopeFactory;

        public RoomService(HttpClient httpClient, IServiceScopeFactory scopeFactory)
        {
            this.httpClient = httpClient;
            this.scopeFactory = scopeFactory;
        }

        // https://stackoverflow.com/questions/592248/how-can-i-check-if-the-current-time-is-between-in-a-time-frame
        // ty u bueaitiful sonnabitch
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

        public async Task<List<Rooms>> GetRooms(
            string? q,
            int? version,
            string? region,
            string? hideEmpty,
            string? hideFull,
            string? hidePassworded,
            string? hideOfficial,
            string? hideCustom,
            string? hideUnranked,
            string? orderBy,
            int? page,
            int? perPage)
        {
            JToken queryRoomResponse = await this.QueryRooms(q, version, region, hideEmpty, hideFull, hidePassworded, hideOfficial, hideCustom, hideUnranked, orderBy, page, perPage);
            RoomObject roomObject = JsonConvert.DeserializeObject<RoomObject>(queryRoomResponse.ToString());
            List<Rooms> rooms = new List<Rooms>();

            foreach (Rooms room in roomObject.Data)
            {
                rooms.Add(room);
            }

            return rooms;
        }

        public async Task<CurrentServerInfo> GetCSIRooms(string? hideEmpty, string? hideFull, string? hidePassworded, string? hideOfficial, string? hideCustom, string? hideUnranked, string? region)
        {
            // Send first request
            JToken queryRoomResponse = await this.QueryRooms(null, null, null, hideEmpty, hideFull, hidePassworded, hideOfficial, hideCustom, hideUnranked, "official%3Adesc", 1, 100);

            RoomObject roomObject = JsonConvert.DeserializeObject<RoomObject>(queryRoomResponse.ToString());

            List<Rooms> rooms = new List<Rooms>();

            int totalPlayers = 0;
            int i = 1;

            foreach (Rooms room in roomObject.Data)
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
            while (rooms.Count != roomObject.TotalCount)
            {
                i++;

                // Send another request for moar data
                queryRoomResponse = await this.QueryRooms(null, null, null, hideEmpty, hideFull, hidePassworded, hideOfficial, hideCustom, hideUnranked, "official%3Adesc", i, 100);

                // Decode the mainframe overloader
                roomObject = JsonConvert.DeserializeObject<RoomObject>(queryRoomResponse.ToString());

                foreach (Rooms room in roomObject.Data)
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

        public async Task<CurrentServerInfo> GetCSIRooms(
            string? q,
            int? version,
            string? region,
            string? hideEmpty,
            string? hideFull,
            string? hidePassworded,
            string? hideOfficial,
            string? hideCustom,
            string? hideUnranked,
            string? orderBy,
            int? page,
            int? perPage)
        {
            JToken queryRoomResponse = await this.QueryRooms(null, null, region, hideEmpty, hideFull, hidePassworded, hideOfficial, hideCustom, hideUnranked, null, 1, 100);
            RoomObject roomObject = JsonConvert.DeserializeObject<RoomObject>(queryRoomResponse.ToString());
            List<Rooms> rooms = new List<Rooms>();

            int totalPlayers = 0;

            foreach (Rooms room in roomObject.Data)
            {
                rooms.Add(room);
                room.RegionFlag = ConvertRegion(room.Region);
                totalPlayers += room.AgentCount;
            }

            CurrentServerInfo csi = new CurrentServerInfo()
            {
                Rooms = rooms,
                PlayerCount = totalPlayers,
            };

            return csi;
        }

        public async Task ArchiveRoomData(List<Rooms> rooms)
        {
            if (rooms.Count != 0)
            {
                using IServiceScope scope = this.scopeFactory.CreateScope();
                using IntruderContext db = scope.ServiceProvider.GetRequiredService<IntruderContext>();

                foreach (Rooms room in rooms)
                {
                    try
                    {
                        room.DBCreatorSteamId = room.Creator.SteamID;
                        room.DBCurrentMap = room.CurrentMap.Id;
                        if (room.Password != null)
                        {
                            room.Passworded = true;
                        }
                        else
                        {
                            room.Passworded = false;
                        }

                        Rooms dbRoom = db.Rooms.Where(x => x.RoomId == room.RoomId).FirstOrDefault();

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
                            dbRoom.DBCurrentMap = room.CurrentMap.Id;
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

        private static CurrentServerInfo CountRegions(List<Rooms> rooms)
        {
            CurrentServerInfo csi = new CurrentServerInfo()
            {
                Rooms = rooms,
            };

            foreach (Rooms regionRooms in rooms)
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

        private async Task<JToken> QueryRooms(
            string? q,
            int? version,
            string? region,
            string? hideEmpty,
            string? hideFull,
            string? hidePassworded,
            string? hideOfficial,
            string? hideCustom,
            string? hideUnranked,
            string? orderBy,
            int? page,
            int? perPage)
        {
            // https://api.intruderfps.com/rooms?
            StringBuilder urlBuilder3000 = new StringBuilder(BaseUrl);

            if (q != null)
            {
                urlBuilder3000.Append($"&Q={q}");
            }

            if (version != null)
            {
                urlBuilder3000.Append($"&Version={version}");
            }

            if (region != null)
            {
                urlBuilder3000.Append($"&Region={region}");
            }

            if (hideEmpty != null)
            {
                urlBuilder3000.Append($"&HideEmpty={hideEmpty}");
            }

            if (hideFull != null)
            {
                urlBuilder3000.Append($"&HideFull={hideFull}");
            }

            if (hidePassworded != null)
            {
                urlBuilder3000.Append($"&HidePassworded={hidePassworded}");
            }

            if (hideOfficial != null)
            {
                urlBuilder3000.Append($"&HideOfficial={hideOfficial}");
            }

            if (hideCustom != null)
            {
                urlBuilder3000.Append($"&HideCustom={hideCustom}");
            }

            if (hideUnranked != null)
            {
                urlBuilder3000.Append($"&HideUnranked={hideUnranked}");
            }

            if (orderBy != null)
            {
                // https://api.intruderfps.com/agents?Q=XXXXXXXXX&OrderBy=XXXX:XXXX
                // https://api.intruderfps.com/agents?&OrderBy=XXXX:XXXX
                urlBuilder3000.Append($"&OrderBy={orderBy}");
            }

            if (page != null)
            {
                if (page == 0)
                {
                    page = 1;
                }

                // https://api.intruderfps.com/agents?Q=XXXXXXXXX&OrderBy=XXXX:XXXX&OnlineOnly=XXXXX&Page=X
                // https://api.intruderfps.com/agents?&Page=X
                urlBuilder3000.Append($"&Page={page}");
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
                string rawJson = await this.httpClient.GetStringAsync(new Uri(urlBuilder3000.ToString()));
                return string.IsNullOrEmpty(rawJson) ? null : JToken.Parse(rawJson);
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, $"Failed to run a query for Agents. URL: {urlBuilder3000}");
                return null;
            }
        }
    }
}
