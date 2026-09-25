using Data.FileManagement;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Models;

namespace Data;

public class ModsFileManager(string path) : IMods
{
    private string _path = path;

    public IEnumerable<WorkshopModDto> GetFromFile()
    {
        var fileContent = FileReader.ReadFromFile(_path);

        return JsonConvert.DeserializeObject<IEnumerable<WorkshopModDto>>(fileContent) ??  Enumerable.Empty<WorkshopModDto>();
    }

    public void SaveToFile(IEnumerable<WorkshopModDto> workshopModDtos)
    {
        FileWriter.WriteToFile(_path, JsonConvert.SerializeObject(workshopModDtos));
    }
}