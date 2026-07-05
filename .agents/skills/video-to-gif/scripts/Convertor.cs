using MediaToolkit;
using MediaToolkit.Model;
using System.IO;
using System.Threading.Tasks;

namespace VideoToGifSkill
{
    public class Mp4VideoConverter : IVideoConverter
    {
        private readonly IOutputDirectoryHelper _dirHelper;
        private readonly IOutputFileHelper _fileHelper;

        public Mp4VideoConverter(IOutputDirectoryHelper dirHelper, IOutputFileHelper fileHelper)
        {
            _dirHelper = dirHelper;
            _fileHelper = fileHelper;
        }

        public string SupportedExtension => ".mp4";

        public async Task<bool> Convert(string input)
        {
            var outputDir = _dirHelper.CheckOrCreateDirectory(input);
            if (string.IsNullOrEmpty(outputDir))
                return false;

            var gifPath = _fileHelper.GetGifName(outputDir, input);
            if (File.Exists(gifPath))
                File.Delete(gifPath);

            var inputFile = new MediaFile { Filename = input };
            var outputFile = new MediaFile { Filename = gifPath };

            using var engine = new Engine();
            engine.GetMetadata(inputFile);
            engine.Convert(inputFile, outputFile);

            await Task.CompletedTask;
            return true;
        }
    }
}