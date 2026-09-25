using Services.Factories;
using Services.Interfaces;
using Services.Models;

namespace Services;

public class SteamManager
{
    private List<WorkshopMod> _workshopItems;
    private IMods _modsFileManager;
    
    public SteamManager(IMods modsFileManager)
    {
        _workshopItems = [];
        _modsFileManager = modsFileManager;
        
        try
        {
            var fileContent = modsFileManager.GetFromFile();
            
            if (fileContent.Any())
            {
                foreach (var workshopMod in fileContent)
                {
                    _workshopItems.Add(WorkshopModFactories.CreateWorkshopModFrom(workshopMod));
                }
            
                return;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    
    public IEnumerable<WorkshopMod> GetWorkshopItems()
    {
        return _workshopItems;
    }

    public void AddWorkshopItem(string workshopItemId)
    {
        _workshopItems.Add(new(workshopItemId));
    }
    
    public async Task UpdateWorkshopItemData()
    {
        if (_workshopItems.Count == 0)
        {
            return;
        }
        
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
            
            // Only update the previousSubscribers if there is a difference in subscribers
            if(workshopItem.Subscribers != dtoItem.Subscriptions)
                workshopItem.PreviousSubscribers  = workshopItem.Subscribers;
            
            workshopItem.Subscribers = dtoItem.Subscriptions;
            workshopItem.LastUpdated = DateTime.Now;
        }
        
        _modsFileManager.SaveToFile(
            _workshopItems.Select(WorkshopModFactories.CreateWorkshopModDtoFrom)
        );
    }
}