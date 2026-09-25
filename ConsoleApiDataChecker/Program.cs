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
        
        SteamManager steamManager = new SteamManager(fileManager);
        
        // Set up api timer
        using var cancellationTokenSource = new CancellationTokenSource();

        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cancellationTokenSource.Cancel();
        };

        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));

        try
        {
            do
            {
                Console.Clear();
                try
                {
                    await steamManager.UpdateWorkshopItemData();

                    WriteDataToScreen(steamManager.GetWorkshopItems());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            } while (await timer.WaitForNextTickAsync(cancellationTokenSource.Token));
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Shutting down...");
        }
    }

    private static void WriteDataToScreen(IEnumerable<WorkshopMod> workshopItems)
    {
        if (workshopItems == null || workshopItems.Count() == 0)
        {
            Console.WriteLine("No workshop items found.");
            return;
        }
        
        foreach (var workshopItem in workshopItems)
        {
            Console.WriteLine($"{workshopItem.Title} with {workshopItem.Subscribers} subscriptions");
        }
    }
}