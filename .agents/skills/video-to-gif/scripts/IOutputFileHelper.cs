namespace VideoToGifSkill;

/// <summary>
/// Provides an abstraction for generating the output GIF file name.
/// </summary>
public interface IOutputFileHelper
{
    /// <summary>
    /// Returns the full path of the GIF file given the output directory and input video file name.
    /// </summary>
    string GetGifName(string outputDir, string inputFileName);
}
