using System.Linq.Expressions;

namespace SortingImageFiles;
public class Program
{
    static int Main(string[] args)
    {
        var extensions = new string[]
        {
            "png","jpg"
        };

        DirectoryInfo directory =null;
        try
        {
            directory = SetRunState(args) switch
            {
                RunState.Costom => new DirectoryInfo(args[0]),
                RunState.Default => new DirectoryInfo(Directory.GetCurrentDirectory())
            };
        }
        catch (Exception e) 
        {
            Print(e.Message, ConsoleColor.Red);
            return 1;
        }

        var fileInfos = directory.GetFiles();

        var dictonryFiles = extensions.ToDictionary(extensions => extensions,  extensions=> new List<FileInfo>());

        foreach (var file in fileInfos)
        {
            foreach (var extension in extensions)
            {
                if (file.Extension == $".{extension}")
                {
                    dictonryFiles[extension].Add(file);
                }
            }
        }

        foreach (var extension in extensions)
        {
            directory.CreateSubdirectory(extension.ToUpper());
        }

        foreach (var (extension, files) in dictonryFiles)
        {
            foreach (var file in files)
            {
                Console.WriteLine($"{directory.FullName}/{extension.ToUpper()}/{file.Name}");// для проверки 
                File.Copy(file.Name, $"{directory.FullName}/{extension.ToUpper()}/{file.Name}");
            }
        }

        return 0;
    }

    private static RunState SetRunState (string[] args)
    {
        if (args.Length == 0)
        {
            return  RunState.Default;
        }
        var directoryName = args[0];
        return Directory.Exists(directoryName) ? RunState.Costom : throw new Exception($"ОШИБКА!!! <{args[0]}> - не существует");
    }

    private static void Print(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}