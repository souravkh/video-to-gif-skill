namespace VideoToGifSkill;

using System.Threading.Tasks;

public interface IVideoConverter
{
    /// <summary>
    /// The file extension (including dot) that this converter supports, e.g., ".mp4".
    /// </summary>
    string SupportedExtension { get; }

    /// <summary>
    /// Converts the specified input video file to a GIF.
    /// Returns true if successful, false otherwise.
    /// </summary>
    Task<bool> Convert(string input);
}
