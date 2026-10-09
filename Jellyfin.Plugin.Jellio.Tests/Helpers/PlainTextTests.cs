using Jellyfin.Plugin.JellioDirect.Helpers;

namespace Jellyfin.Plugin.JellioDirect.Tests.Helpers;

public class PlainTextTests
{
    [Fact]
    public void FromHtml_TurnsLineBreaksIntoNewlinesAndDropsTags()
    {
        Assert.Equal(
            "Tsuneo meets Josee.\n\n(Source: Anime News Network)",
            PlainText.FromHtml("Tsuneo meets Josee.<br><br>(Source: Anime News Network)"));
        Assert.Equal("Line one\nLine two", PlainText.FromHtml("Line one<BR/>Line two"));
        Assert.Equal("A title in italics.", PlainText.FromHtml("A <i>title</i> in italics.<i/>"));
    }

    [Fact]
    public void FromHtml_DecodesEntitiesAndCollapsesExtraBlankLines()
    {
        Assert.Equal("Tom & Jerry \"live\"", PlainText.FromHtml("Tom &amp; Jerry &quot;live&quot;"));
        Assert.Equal("One\n\nTwo", PlainText.FromHtml("One<br><br><br><br>Two"));
    }

    [Fact]
    public void FromHtml_LeavesPlainTextAlone()
    {
        Assert.Null(PlainText.FromHtml(null));
        Assert.Equal(string.Empty, PlainText.FromHtml(string.Empty));
        Assert.Equal("If a < b and c > d, nothing changes.", PlainText.FromHtml("If a < b and c > d, nothing changes."));
    }
}
