using System.Text.Json.Serialization;

namespace Services.Models;

public record SteamWorkshopResponseDTO
{
    [JsonPropertyName("response")]
    public SteamResponseDTO Response { get; set; }
}

public record SteamResponseDTO
{
    [JsonPropertyName("publishedfiledetails")]
    public List<WorkshopItemDTO> PublishedFileDetails { get; set; }
}

public record WorkshopItemDTO
{
    [JsonPropertyName("publishedfileid")]
    public string PublishedFileId { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("preview_url")]
    public string PreviewUrl { get; set; }

    [JsonPropertyName("subscriptions")]
    public int Subscriptions { get; set; }
}

public record WorkshopModDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("imageUrl")]
    public string ImageUrl { get; set; }
    [JsonPropertyName("subscribers")]
    public int Subscribers { get; set; }
    [JsonPropertyName("previousSubscribers")]
    public int PreviousSubscribers { get; set; }
    [JsonPropertyName("lastUpdated")]
    public DateTime LastUpdated { get; set; }
}