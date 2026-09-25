using Services.Factories;
using Services.Interfaces;
using Services.Models;

namespace Services;

public class SteamManager
{
    private List<WorkshopMod> _workshopItems;
    private IMods _modsFileManager;
    
    public SteamManager(string[] workshopIds, IMods modsFileManager)
    {
        _workshopItems = [];
        _modsFileManager = modsFileManager;

        var fileContent = modsFileManager.GetFromFile();

        
        if (fileContent.Any())
        {
            foreach (var workshopMod in fileContent)
            {
                _workshopItems.Add(WorkshopModFactories.CreateWorkshopModFrom(workshopMod));
            }
            
            return;
        }
        
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
        
        _modsFileManager.SaveToFile(
            _workshopItems.Select(WorkshopModFactories.CreateWorkshopModDtoFrom)
        );
    }
}