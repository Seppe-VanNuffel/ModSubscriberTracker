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

    public WorkshopMod(string id, string title, string imageUrl, int subscribers, int previousSubscribers, DateTime lastUpdated)
    {
        Id = id;
        Title = title;
        ImageUrl = imageUrl;
        Subscribers = subscribers;
        PreviousSubscribers = previousSubscribers;
        LastUpdated = lastUpdated;
    }

    public string ComparisonText()
    {
        if(Subscribers > PreviousSubscribers)
            return $"▲ +{Subscribers-PreviousSubscribers}";
        else if (Subscribers == PreviousSubscribers)
            return "-";
        else
            return $"▼ -{PreviousSubscribers-Subscribers}";
    }
}