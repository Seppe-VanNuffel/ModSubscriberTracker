using Data;
using Services;
using Services.Interfaces;
using Services.Models;

namespace ConsoleApiDataChecker;

class Program
{
    static async Task Main(string[] args)
    {
        IMods fileManager = new ModsFileManager(@"C:\test\Mods.json");
        
        string[] workshopIds = ["3802185577", "3721437118"];

        SteamManager steamManager = new SteamManager(workshopIds, fileManager);

        //steamManager.RestoreSessionData();
        
        while (true)
        {
            Console.Clear();

            await steamManager.UpdateWorkshopItemData();

            WriteDataToScreen(steamManager.GetWorkshopItems());
            
            Thread.Sleep(60000);
        }
    }

    private static void WriteDataToScreen(IEnumerable<WorkshopMod> workshopItems)
    {
        foreach (var workshopItem in workshopItems)
        {
            Console.WriteLine($"{workshopItem.Title} with {workshopItem.Subscribers} subscriptions");
        }
    }
}