using Services.Models;

namespace Services.Factories;

public static class WorkshopModFactories
{
    public static WorkshopMod CreateNewWorkshopMod(string id)
    {
        WorkshopMod mod = new WorkshopMod(id);

        return mod;
    }

    public static WorkshopMod CreateWorkshopModFrom(WorkshopModDto workshopModDto)
    {
        return new WorkshopMod(
                workshopModDto.Id,
                workshopModDto.Title,
                workshopModDto.ImageUrl,
                workshopModDto.Subscribers,
                workshopModDto.PreviousSubscribers,
                workshopModDto.LastUpdated
        );
    }

    public static WorkshopModDto CreateWorkshopModDtoFrom(WorkshopMod mod)
    {
        return new WorkshopModDto
        {
            Id = mod.Id,
            Title = mod.Title,
            ImageUrl = mod.ImageUrl,
            Subscribers = mod.Subscribers,
            PreviousSubscribers = mod.PreviousSubscribers,
            LastUpdated = mod.LastUpdated
        };
    }
}