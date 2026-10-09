using Jellyfin.Plugin.Jellio.Controllers;
using Jellyfin.Plugin.Jellio.Models;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.Jellio.Tests.Controllers;

public class AddonControllerStreamDeliveryTests
{
    private static readonly Guid ItemId = Guid.Parse("88cf5e8e-f21a-af3f-c857-95c583d754fe");

    [Theory]
    [InlineData(null, "both")]
    [InlineData("", "both")]
    [InlineData("bogus", "both")]
    [InlineData("direct", "direct")]
    [InlineData("hls", "hls")]
    [InlineData("both", "both")]
    public void NormalizeStreamDeliveryMode_DefaultsToBoth(string? mode, string expected)
    {
        Assert.Equal(expected, AddonController.NormalizeStreamDeliveryMode(mode));
    }

    [Theory]
    // delivery, video, audio, expect direct, expect HLS
    [InlineData("both", "adaptive", "adaptive", true, true)]
    [InlineData("direct", "adaptive", "adaptive", true, false)]
    [InlineData("hls", "adaptive", "adaptive", false, true)]
    [InlineData("hls", "force", "disabled", false, true)]
    // HLS with nothing to transcode falls back to the original file.
    [InlineData("hls", "disabled", "disabled", true, false)]
    [InlineData("both", "disabled", "disabled", true, false)]
    public void StreamSelection_FollowsDeliveryAndTranscodingModes(
        string delivery,
        string video,
        string audio,
        bool expectDirect,
        bool expectHls)
    {
        Assert.Equal(expectDirect, AddonController.ShouldIncludeDirectStream(delivery, video, audio));
        Assert.Equal(expectHls, AddonController.ShouldIncludeHlsStreams(delivery, video, audio));
    }

    [Fact]
    public void BuildDirectStreamUrl_RequestsStaticFileWithBothTokenParameters()
    {
        var url = AddonController.BuildDirectStreamUrl(
            "https://jellyfin.example",
            ItemId,
            "88cf5e8ef21aaf3fc85795c583d754fe",
            "token123");

        Assert.Equal(
            "https://jellyfin.example/Videos/88cf5e8e-f21a-af3f-c857-95c583d754fe/stream"
                + "?static=true&mediaSourceId=88cf5e8ef21aaf3fc85795c583d754fe&ApiKey=token123&api_key=token123",
            url);
    }

    [Fact]
    public void BuildSubtitleUrl_UsesRouteWithoutStartTicksAndBothTokenParameters()
    {
        var url = AddonController.BuildSubtitleUrl(
            "https://jellyfin.example",
            ItemId,
            "88cf5e8ef21aaf3fc85795c583d754fe",
            5,
            "ass",
            "token123");

        Assert.Equal(
            "https://jellyfin.example/Videos/88cf5e8e-f21a-af3f-c857-95c583d754fe/88cf5e8ef21aaf3fc85795c583d754fe"
                + "/Subtitles/5/Stream.ass?api_key=token123&ApiKey=token123",
            url);
    }

    [Fact]
    public void SubtitlesForDirectStream_KeepsOnlySubtitleFilesNextToTheVideo()
    {
        var streams = new[]
        {
            new MediaStream { Index = 3, IsExternal = false },
            new MediaStream { Index = 4, IsExternal = false },
            new MediaStream { Index = 0, IsExternal = true },
        };
        var subtitles = streams
            .Select(s => new SubtitleDto { Id = $"sub-{s.Index}", Url = $"https://jf/{s.Index}.srt", Lang = "eng" })
            .ToList();

        var direct = AddonController.SubtitlesForDirectStream(streams, subtitles);

        Assert.Equal(new[] { "sub-0" }, direct.Select(s => s.Id));
    }
}
