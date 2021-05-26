namespace Bloon.Features.Censor
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    // Copy-pa...derived from http://james.newtonking.com/archive/2009/07/03/simple-net-profanity-filter
    public class Censorer
    {
        private readonly Dictionary<int, Regex> censorRegexes = new Dictionary<int, Regex>();

        public void Init(List<Censor> censors)
        {
            censors.ForEach((Censor censor) => this.AddCensor(censor));
        }

        public void Reset()
        {
            this.censorRegexes.Clear();
        }

        public void AddCensor(Censor censor)
        {
            this.censorRegexes.Add(censor.Id, new Regex(ToRegexPattern(censor.Pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));
        }

        public void RemoveCensor(int censorId)
        {
            this.censorRegexes.Remove(censorId);
        }

        public bool TryCensorContent(string text, out string highlighted, out KeyValuePair<int, Regex> censor)
        {
            highlighted = text;
            censor = default;

            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            foreach (KeyValuePair<int, Regex> censorRegex in this.censorRegexes)
            {
                if (censorRegex.Value.IsMatch(text))
                {
                    highlighted = censorRegex.Value.Replace(text, (Match match) => $"**{match.Value}**");
                    censor = censorRegex;
                    return true;
                }
            }

            return false;
        }

        private static string ToRegexPattern(string pattern)
        {
            return @"\b" + pattern + @"\b";
        }
    }
}
