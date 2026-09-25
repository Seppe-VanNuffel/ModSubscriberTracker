using System.Text.Json.Serialization;

namespace Services.Models;

public class SteamWorkshopResponse
{
    [JsonPropertyName("response")]
    public SteamResponse Response { get; set; }
}

public class SteamResponse
{
    [JsonPropertyName("publishedfiledetails")]
    public List<WorkshopItem> PublishedFileDetails { get; set; }
}

public class WorkshopItem
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("preview_url")]
    public string PreviewUrl { get; set; }

    [JsonPropertyName("subscriptions")]
    public int Subscriptions { get; set; }
}