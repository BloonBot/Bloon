namespace Bloon.Features.IntruderBackend.Tuning
{
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// https://api.intruderfps.com/tuning .
    /// </summary>
    public class TuningObject
    {
        public int Id { get; set; }

        public JObject Raw { get; set; }

        public bool Live { get; set; }
    }
}
