namespace VideoToGifSkill;
using System.Diagnostics;

public class Program
{


    static bool IsFfmpegAvailable()
{
    try
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = "-version",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        process.WaitForExit();

        return process.ExitCode == 0;
    }
    catch
    {
        return false;
    }
}


public static async Task Main(string[] args)
    {

        // Print ASCII art header
        Console.WriteLine(@"\n __      __   _          _   _             \n \\ \    / /__| |__   ___| |_| |_ ___ _ __ \n  \\ \/\/ / -_) '_ \\ / _ \\  _|  _/ -_) '__|\n   \\_/\\_/\\___|_.__/ \\___/\\__|\\__\\___|_|   \n");

        // Validate arguments
        if (args?.Length < 1)
            throw new Exception("Please provide a valid input location/arguments");

        string input = args[0];

        if (string.IsNullOrWhiteSpace(input))
            throw new Exception("Please provide a valid input location");

        bool isFile = File.Exists(input);
        bool isDirectory = Directory.Exists(input);

        if (!isFile && !isDirectory)
            throw new Exception("The input argument is not valid!");

        Mp4Convertor mp4Convertor = new Mp4Convertor();
        var outputPaths = new List<string>();
        bool result = false;
        
        if (isFile)
        {
            result = await (mp4Convertor?.Convert(input) ?? Task.FromResult(false));
            if (result)
            {
                string outDir = OutputDirectoryHelper.CheckOrCreateDirectory(input);
                string gifPath = Path.Combine(outDir, Path.GetFileNameWithoutExtension(input) + ".gif");
                outputPaths.Add(gifPath);
            }
        }
        else // directory
        {
            IEnumerable<string> mp4Files = Enumerable.Empty<string>();
            try
            {
                mp4Files = Directory.EnumerateFiles(input, "*.mp4", SearchOption.AllDirectories);
            }
            catch (UnauthorizedAccessException)
            {
                // Skip inaccessible directories
                mp4Files = Enumerable.Empty<string>();
            }

            // Order files by size descending
            var orderedFiles = mp4Files
                .Select(f => new FileInfo(f))
                .OrderByDescending(fi => fi.Length)
                .Select(fi => fi.FullName);

            foreach (var mp4File in orderedFiles)
                {
                    result = await (mp4Convertor?.Convert(mp4File) ?? Task.FromResult(false));
                if (result)
                {
                    string outDir = OutputDirectoryHelper.CheckOrCreateDirectory(mp4File);
                    string gifPath = Path.Combine(outDir, Path.GetFileNameWithoutExtension(mp4File) + ".gif");
                    outputPaths.Add(gifPath);
                }
            }
        }

        // Show generated GIF names with locations
        Console.WriteLine("\nGenerated GIFs:");
        foreach (var path in outputPaths)
        {
            Console.WriteLine(path);
        }

        // Print END! ASCII art
        Console.WriteLine(@"\n  _____ _   _ _____ \n | ____| \ | |_   _|\n |  _| |  \| | | |  \n | |___| |\\  | | |  \n |_____|_| \_| |_|  \n");
    }
}