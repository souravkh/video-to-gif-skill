namespace VideoToGifSkill;
public static class OutputDirectoryHelper
{
    public const string OUTPUT ="Output";

    public static string CheckOrCreateDirectory(string input)
    {
        if (!string.IsNullOrWhiteSpace(input))
        {
            string inputDir = Path.GetDirectoryName(input);
            string outputDir= Path.Combine(inputDir,OUTPUT);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
            return outputDir;
        }
        return string.Empty;
    }
} 