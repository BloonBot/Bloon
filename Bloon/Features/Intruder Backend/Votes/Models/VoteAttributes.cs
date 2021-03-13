namespace Bloon.Features.IntruderBackend.Votes
{
    using System.ComponentModel.DataAnnotations.Schema;

    [NotMapped]
    public class VoteAttributes
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
