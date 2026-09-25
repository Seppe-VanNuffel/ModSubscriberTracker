using Services.Models;

namespace Services.Factories;

public static class WorkshopModFactories
{
    public static WorkshopMod CreateNewWorkshopMod(string id)
    {
        WorkshopMod mod = new WorkshopMod(id);

        return mod;
    }
}