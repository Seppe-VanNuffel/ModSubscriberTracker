using Newtonsoft.Json;
using Services.Models;

namespace Data.FileManagement;

public static class FileReader
{
    public static string ReadFromFile(string path)
    {
        string fileContent = string.Empty;
        
        try
        {
            fileContent = File.ReadAllText(path);
        }
        catch (Exception e)
        {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }

        return fileContent;
    }
}