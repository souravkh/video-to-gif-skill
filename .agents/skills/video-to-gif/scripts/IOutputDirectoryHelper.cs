namespace VideoToGifSkill;

/// <summary>
/// Provides an abstraction for retrieving/creating the output directory.
/// </summary>
public interface IOutputDirectoryHelper
{
    /// <summary>
    /// Returns the output directory for a given input file, creating it if missing.
    /// Returns an empty string if the input is invalid.
    /// </summary>
    string CheckOrCreateDirectory(string input);
}
