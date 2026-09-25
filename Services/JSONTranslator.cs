using Newtonsoft.Json;

namespace Services;

public static class JsonTranslator
{
    public static Models.SteamWorkshopResponseDTO TranslateSteamJson(string jsonData)
    {
        return JsonConvert.DeserializeObject<Models.SteamWorkshopResponseDTO>(jsonData) ?? throw new InvalidOperationException();
    }
}