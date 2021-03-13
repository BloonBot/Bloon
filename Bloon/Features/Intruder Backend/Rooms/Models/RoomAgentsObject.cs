namespace Bloon.Features.IntruderBackend.Rooms
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bloon.Features.IntruderBackend.Agents;

    /// <summary>
    /// https://api.intruderfps.com/rooms/####/agents .
    /// </summary>
    [NotMapped]
    public class RoomAgentsObject
    {
        public List<Agent> AgentsInRoom { get; set; }
    }
}
