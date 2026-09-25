namespace Services.Models;

public class WorkshopMod
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public int Subscribers { get; set; }
    public int PreviousSubscribers { get; set; }
    public DateTime LastUpdated { get; set; }

    public WorkshopMod(string id)
    {
        Id = id;
        Title = string.Empty;
        ImageUrl = string.Empty;
        Subscribers  = 0;
        PreviousSubscribers = 0;
        LastUpdated = DateTime.Now;
    }
}