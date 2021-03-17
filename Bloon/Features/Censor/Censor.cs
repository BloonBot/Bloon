namespace Bloon.Features.Censor
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    // Copy-pa...derived from http://james.newtonking.com/archive/2009/07/03/simple-net-profanity-filter
    public class Censor
    {
        private readonly List<Regex> censorRegexes;

        public Censor(IEnumerable<string> censoredWords)
        {
            this.censorRegexes = censoredWords.Select(x => new Regex(ToRegexPattern(x), RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant)).ToList();
        }

        public string CensorText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            string censoredText = text;

            foreach (Regex censorRegex in this.censorRegexes)
            {
                censoredText = censorRegex.Replace(censoredText, StarCensoredMatch);
            }

            return censoredText;
        }

        public bool HasNaughtyWord(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            foreach (Regex censoredWord in this.censorRegexes)
            {
                if (censoredWord.IsMatch(text))
                {
                    return true;
                }
            }

            return false;
        }

        private static string StarCensoredMatch(Match m) => new ('*', m.Captures[0].Value.Length);

        private static string ToRegexPattern(string wordRegex) => @"\b" + wordRegex + @"\b";
    }
}
