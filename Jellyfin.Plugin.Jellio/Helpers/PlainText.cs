using System.Net;
using System.Text.RegularExpressions;

namespace Jellyfin.Plugin.JellioDirect.Helpers;

/// <summary>
/// Turns the HTML some metadata providers put in overviews (line breaks, italics, entities) into
/// plain text. Stremio clients show descriptions as text, so the tags would appear literally.
/// </summary>
public static partial class PlainText
{
    public static string? FromHtml(string? html)
    {
        if (string.IsNullOrEmpty(html))
        {
            return html;
        }

        var text = LineBreak().Replace(html, "\n");
        text = Tag().Replace(text, string.Empty);
        text = WebUtility.HtmlDecode(text);
        text = ExtraBlankLines().Replace(text, "\n\n");
        return text.Trim();
    }

    [GeneratedRegex(@"<\s*br\s*/?\s*>", RegexOptions.IgnoreCase)]
    private static partial Regex LineBreak();

    // Only real tags (a letter after "<" or "</"), so text like "a < b" is left alone.
    [GeneratedRegex(@"</?[a-zA-Z][^<>]*>")]
    private static partial Regex Tag();

    [GeneratedRegex(@"\n{3,}")]
    private static partial Regex ExtraBlankLines();
}
