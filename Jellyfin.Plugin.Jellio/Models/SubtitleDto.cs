using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.Jellio.Models;

public class SubtitleDto
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    /// <summary>
    /// Gets or sets the language code (e.g. "tha"); clients map it to a language name.
    /// </summary>
    [JsonPropertyName("lang")]
    public required string Lang { get; set; }

    /// <summary>
    /// Gets or sets the track title (e.g. "SDH"). Not part of the Stremio protocol; clients that
    /// don't know it ignore it.
    /// </summary>
    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Label { get; set; }
}
