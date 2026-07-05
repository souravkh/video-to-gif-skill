namespace VideoToGifSkill;

using System.Diagnostics;

/// <summary>
/// Entry point for the Video to GIF CLI application.
/// Validates the input, converts one or more MP4 files to GIFs,
/// and displays the generated output paths.
/// </summary>
public class Program
{
    #region FFmpeg Validation

    /// <summary>
    /// Determines whether FFmpeg is installed and available in the system PATH.
    /// </summary>
    /// <returns>
    /// <c>true</c> if FFmpeg is available; otherwise, <c>false</c>.
    /// </returns>
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

    #endregion

    #region Application Entry Point

    /// <summary>
    /// Application entry point.
    /// Converts a single MP4 file or all MP4 files within a directory
    /// into GIF images.
    /// </summary>
    /// <param name="args">
    /// Command-line arguments.
    /// The first argument must be a valid file or directory path.
    /// </param>
    public static async Task Main(string[] args)
    {
        // Print ASCII art header
        Console.WriteLine(@"\n __      __   _          _   _             
 \\ \    / /__| |__   ___| |_| |_ ___ _ __ 
  \\ \/\/ / -_) '_ \\ / _ \\  _|  _/ -_) '__|
   \\_/\\_/\\___|_.__/ \\___/\\__|\\__\\___|_|   
");

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

        IOutputDirectoryHelper dirHelper = new OutputDirectoryHelper();
        IOutputFileHelper fileHelper = new OutputFileHelper();
        IVideoConverter mp4Converter = new Mp4VideoConverter(dirHelper, fileHelper);
        ConverterFactory factory = new ConverterFactory(new IVideoConverter[] { mp4Converter });
        IList<string> outputPaths = new List<string>();
        bool result = false;

        if (isFile)
        {
            IVideoConverter converter = factory.GetConverter(input);
            result = await converter.Convert(input);
            if (result)
            {
                string outDir = dirHelper.CheckOrCreateDirectory(input);
                string gifPath = fileHelper.GetGifName(outDir, input);
                outputPaths.Add(gifPath);
            }
        }
        else
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

            IEnumerable<string> orderedFiles = mp4Files
                .Select(f => new FileInfo(f))
                .OrderByDescending(fi => fi.Length)
                .Select(fi => fi.FullName);

            foreach (var mp4File in orderedFiles)
            {
                IVideoConverter converter = factory.GetConverter(mp4File);
                result = await converter.Convert(mp4File);
                if (result)
                {
                    string outDir = dirHelper.CheckOrCreateDirectory(mp4File);
                    string gifPath = fileHelper.GetGifName(outDir, mp4File);
                    outputPaths.Add(gifPath);
                }
            }
        }

        Console.WriteLine("\nGenerated GIFs:");
        foreach (string path in outputPaths)
        {
            Console.WriteLine(path);
        }

        Console.WriteLine(@"\n  _____ _   _ _____ 
  | ____| \\ | |_   _|
  |  _| |  \\| | | |  
  | |___| |\\\\  | | |  
  |_____|_| \\_| |_|  ");
    }

    #endregion
}