namespace VideoToGifSkill;

/// <summary>
/// Provides helper methods for generating output file names for converted GIFs.
/// </summary>
public static class OutputFileHelper
{
    #region Constants

    /// <summary>
    /// File extension used for generated GIF files.
    /// </summary>
    public const string GIF_EXTENSION = ".gif";

    #endregion

    #region Public Methods

    /// <summary>
    /// Generates the full output path for a GIF based on the input file name.
    /// </summary>
    /// <param name="outputDir">
    /// The directory where the GIF will be created.
    /// </param>
    /// <param name="inputFileName">
    /// The original input video file path or name.
    /// </param>
    /// <returns>
    /// The full path of the generated GIF file.
    /// </returns>
    public static string GetGIFName(string outputDir, string inputFileName)
    {
        string fileName = Path.GetFileNameWithoutExtension(inputFileName);
        return Path.Combine(outputDir, fileName + GIF_EXTENSION);
    }

    #endregion
}