namespace VideoToGifSkill;

using MediaToolkit.Model;
using MediaToolkit;

public class Mp4Convertor
{

    public async Task<bool> Convert(string input)
    {

        string outputDir = OutputDirectoryHelper.CheckOrCreateDirectory(input);
        
        if (string.IsNullOrEmpty(outputDir))
            return false;
        
        string fileName = OutputFileHelper.GetGIFName(outputDir, input);

        if(File.Exists(fileName))
            File.Delete(fileName);


        MediaFile inputFile = new MediaFile { Filename = input };
        MediaFile outputFile = new MediaFile { Filename = fileName };

        using (Engine engine = new Engine())
        {
            engine.GetMetadata(inputFile);
            engine.Convert(inputFile, outputFile);
        }
        return true;
        
    }
}
