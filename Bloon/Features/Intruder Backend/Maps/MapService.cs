//namespace Bloon.Features.IntruderBackend.Servers
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Net.Http;
//    using System.Text;
//    using System.Threading.Tasks;
//    using Bloon.Core.Database;
//    using Bloon.Features.IntruderBackend.Agents;
//    using Bloon.Features.IntruderBackend.Maps;
//    using Bloon.Features.IntruderBackend.Rooms;
//    using Bloon.Variables.Emojis;
//    using Microsoft.Extensions.DependencyInjection;
//    using Newtonsoft.Json;
//    using Newtonsoft.Json.Linq;
//    using Serilog;

//    public partial class RoomService
//    {
//        public async Task ArchiveRoomMapHistory(List<Rooms> rooms)
//        {
//            if (rooms.Count != 0)
//            {
//                using IServiceScope scope = this.scopeFactory.CreateScope();
//                using AccountsContext db = scope.ServiceProvider.GetRequiredService<AccountsContext>();

//                foreach (Rooms room in rooms)
//                {
//                    try
//                    {
//                        Map roomMap = new Map()
//                        {
//                            IntruderId = room.CurrentMap.Id,
//                            Author = room.CurrentMap.Author,
//                            Name = room.CurrentMap.Name,
//                            Gamemode = room.CurrentMap.Gamemode,
//                            ThumbnailUrl = room.CurrentMap.ThumbnailUrl,
//                            IsMapMakerMap = room.CurrentMap.IsMapMakerMap,
//                            LastUpdate = room.CurrentMap.LastUpdate,
//                            PlayCount = 0,
//                        };

//                        Map dbMap = db.Maps.Where(x => x.IntruderId == roomMap.IntruderId).FirstOrDefault();

//                        if (dbMap == null)
//                        {
//                            db.Maps.Add(roomMap);
//                        }
//                        else
//                        {
//                            dbMap.LastUpdate = roomMap.LastUpdate;
//                            dbMap.PlayCount = dbMap.PlayCount + 1;
//                            db.Maps.Update(dbMap);
//                        }

//                        await db.SaveChangesAsync().ConfigureAwait(false);
//                    }
//                    catch (Exception e1)
//                    {
//                        Log.Error(e1, $"ROOM ID: {room.RoomId} | MAP ID: {room.CurrentMap.Id} | MAP NAME: {room.CurrentMap.Name}");
//                    }
//                }
//            }
//            else
//            {
//                await Task.CompletedTask.ConfigureAwait(false);
//            }
//        }
//    }
//}
