namespace Bloon.Features.Wiki.Models
{
    public class Query
    {
        public RecentChanges[] recentchanges { get; set; }

        public Pages pages { get; set; }

        public Allusers[] allusers { get; set; }

        public Allcategory[] allcategories { get; set; }
    }
}
