namespace Bloon.Features.IntruderBackend.Votes
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Bloon.Features.IntruderBackend.Agents;

    [NotMapped]
    public class VotesObject
    {
        public AgentVotes[] Votes { get; set; }
    }
}
