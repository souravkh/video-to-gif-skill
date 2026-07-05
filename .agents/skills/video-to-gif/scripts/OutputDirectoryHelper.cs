namespace VideoToGifSkill;

/// <summary>
/// Provides helper methods for creating and retrieving the output directory
/// used to store generated GIF files.
/// </summary>
public static class OutputDirectoryHelper
{
    #region Constants

    /// <summary>
    /// Name of the directory where generated GIF files are stored.
    /// </summary>
    public const string Output = "Output";

    #endregion

    #region Public Methods

    /// <summary>
    /// Gets the output directory for the specified input file.
    /// Creates the directory if it does not already exist.
    /// </summary>
    /// <param name="input">
    /// The full path to the input video file.
    /// </param>
    /// <returns>
    /// The full path to the output directory, or <see cref="string.Empty"/>
    /// if the input path is invalid.
    /// </returns>
    public static string CheckOrCreateDirectory(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        string? inputDir = Path.GetDirectoryName(input);

        if (string.IsNullOrWhiteSpace(inputDir))
            return string.Empty;

        string outputDir = Path.Combine(inputDir, Output);

        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        return outputDir;
    }

    #endregion
}