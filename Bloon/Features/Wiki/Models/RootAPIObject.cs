namespace Bloon.Features.Wiki.Models
{
    public class RootAPIObject
    {
        public string batchcomplete { get; set; }

        public APIContinue _continue { get; set; }

        public Query query { get; set; }

        public Parse parse { get; set; }
    }
}
