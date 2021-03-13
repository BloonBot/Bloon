namespace Bloon.Features.IntruderBackend.Votes
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bloon.Features.IntruderBackend.Agents;

    [NotMapped]
    public class VotesObject
    {
        public List<AgentVotes> Votes { get; set; }
    }
}
