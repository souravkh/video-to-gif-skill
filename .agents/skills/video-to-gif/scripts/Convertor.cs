namespace VideoToGifSkill;

using MediaToolkit;
using MediaToolkit.Model;

/// <summary>
/// Provides functionality to convert MP4 video files into GIF images.
/// </summary>
public class Mp4Convertor
{
    #region Public Methods

    /// <summary>
    /// Converts the specified MP4 video into a GIF.
    /// The GIF is created inside the Output directory located next to the input file.
    /// </summary>
    /// <param name="input">
    /// Full path of the input MP4 file.
    /// </param>
    /// <returns>
    /// <c>true</c> if the conversion succeeds; otherwise, <c>false</c>.
    /// </returns>
    public async Task<bool> Convert(string input)
    {
        string outputDir = OutputDirectoryHelper.CheckOrCreateDirectory(input);

        if (string.IsNullOrEmpty(outputDir))
            return false;

        string fileName = OutputFileHelper.GetGIFName(outputDir, input);

        if (File.Exists(fileName))
            File.Delete(fileName);

        MediaFile inputFile = new MediaFile
        {
            Filename = input
        };

        MediaFile outputFile = new MediaFile
        {
            Filename = fileName
        };

        using (Engine engine = new Engine())
        {
            engine.GetMetadata(inputFile);
            engine.Convert(inputFile, outputFile);
        }

        await Task.CompletedTask;
        return true;
    }

    #endregion
}