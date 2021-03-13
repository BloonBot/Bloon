namespace Bloon.Features.Helprace
{
    using System.Text.RegularExpressions;
    using System.Web;

    public static class Sanitize
    {
        private static readonly Regex HtmlTagRegex = new Regex("<.*?>", RegexOptions.Compiled);

        /// <summary>
        /// Removes page breaks or line breaks given HTML.
        /// </summary>
        /// <param name="input">HTML that needs sanitized.</param>
        /// <returns>Clean HTML without page/line breaks.</returns>
        public static string RemoveBreaks(string input) => HttpUtility.HtmlDecode(HtmlTagRegex.Replace(input, string.Empty));
    }
}
