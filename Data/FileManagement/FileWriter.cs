namespace Data.FileManagement;

public static class FileWriter
{
    public static void WriteToFile(string path, string content)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            
            File.WriteAllText(path, content);
        }
        catch (Exception e)
        {
            Console.WriteLine("error writing to " + path);
            Console.WriteLine(e.Message);
        }
    }
}