using Services;
using Newtonsoft.Json;
using Services.Models;

namespace ConsoleApiDataChecker;

class Program
{
    static async Task Main(string[] args)
    {
        string[] workshopIds = ["3802185577", "3721437118"];
        
        string jsonData = await SteamWorkshopService.GetModData(workshopIds);
        
        SteamWorkshopResponse responseData = JsonConvert.DeserializeObject<Services.Models.SteamWorkshopResponse>(jsonData) ??
                                    throw new NullReferenceException();

        foreach (WorkshopItem workshopItem in responseData.Response.PublishedFileDetails)
        {
            Console.WriteLine($"{workshopItem.Title} with {workshopItem.Subscriptions} subscriptions");
        }
    }
}