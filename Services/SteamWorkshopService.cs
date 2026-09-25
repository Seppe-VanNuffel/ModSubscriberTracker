namespace Services;

public static class SteamWorkshopService
{
    private static HttpClient _httpClient = new HttpClient();

    public static async Task<string> GetModData(List<string> WorkshopIds)
    {
        const string url =
            "https://api.steampowered.com/ISteamRemoteStorage/GetPublishedFileDetails/v1/";

        var values = new Dictionary<string, string> {
            { "itemcount", $"{WorkshopIds.Count}" }
        };

        for (int i = 0; i < WorkshopIds.Count; i++)
        {
            values.Add($"publishedfileids[{i}]", WorkshopIds[i]);
        }

        using var content = new FormUrlEncodedContent(values);
        
        using var response = await _httpClient.PostAsync(url, content);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}