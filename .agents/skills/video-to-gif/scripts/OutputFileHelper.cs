namespace VideoToGifSkill;

public static class OutputFileHelper
{
    public const string GIF_EXTENSION=".gif";
    public static string GetGIFName(string outputDir, string inputFileName)
    {
        string fileName = Path.GetFileNameWithoutExtension(inputFileName);
        return Path.Combine(outputDir,fileName+GIF_EXTENSION);
    }
}