using Services.Models;

namespace Services.Interfaces;

public interface IMods
{
    public IEnumerable<WorkshopModDto> GetFromFile();
    
    public void SaveToFile(IEnumerable<WorkshopModDto> workshopModDtos);
}