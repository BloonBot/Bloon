namespace Bloon.Features.Wiki
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Bloon.Utils;

    public static class WikiUtils
    {
        private static readonly Regex SectionHeaderRegex = new (@"==[A-Za-z\s]+==", RegexOptions.Compiled);
        private static readonly Regex WikiLinkRegex = new (@"\[{2}([A-Za-z0-9\s]+)\]{2}", RegexOptions.Compiled);

        public static Uri GetUrlFromTitle(string pageTitle) => new ($"https://wiki.superbossgames.com/wiki/index.php?title={GetWikifiedTitle(pageTitle)}");

        public static string GetWikifiedTitle(string pageTitle) => string.Join("_", pageTitle.Split(' ').Select(x => x.Capitalize()));

        public static string ToMarkdown(string body)
        {
            body = body.Replace("\n", string.Empty, StringComparison.Ordinal); // Remove new lines
            body = SectionHeaderRegex.Replace(body, string.Empty); // Strip out the section header

            // Convert wiki links to markdown links
            // [[Page Title]] -> [Page Title](Link to Page)
            return WikiLinkRegex.Replace(body, (Match m) => $"[{m.Groups[1].Value}]({GetUrlFromTitle(m.Groups[1].Value)})");
        }
    }
}
