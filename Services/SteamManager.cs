using Services.Factories;
using Services.Models;

namespace Services;

public class SteamManager
{
    private List<WorkshopMod> _workshopItems;
    
    public SteamManager(string[] workshopIds)
    {
        _workshopItems = [];
        foreach (var workshopId in workshopIds)
        {
            _workshopItems.Add(WorkshopModFactories.CreateNewWorkshopMod(workshopId));
        }
    }
    
    public IEnumerable<WorkshopMod> GetWorkshopItems()
    {
        return _workshopItems;
    }

    public async Task UpdateWorkshopItemData()
    {
        string jsonData = await SteamWorkshopService.GetModData(_workshopItems.Select(x => x.Id).ToList());

        SteamWorkshopResponseDTO responseDtoData = JsonTranslator.TranslateSteamJson(jsonData);

        foreach (var dtoItem in responseDtoData.Response.PublishedFileDetails)
        {
            WorkshopMod? workshopItem = _workshopItems
                .FirstOrDefault(x => x.Id == dtoItem.PublishedFileId);

            if (workshopItem == null)
                continue;

            workshopItem.Title = dtoItem.Title;
            workshopItem.ImageUrl = dtoItem.PreviewUrl;
            workshopItem.PreviousSubscribers  = workshopItem.Subscribers;
            workshopItem.Subscribers = dtoItem.Subscriptions;
            workshopItem.LastUpdated = DateTime.Now;
        }
    }
}