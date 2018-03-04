using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace YTMusicDL
{
    class AudioManager
    {
        public async static Task ToAudio(string videoTitle, string temporaryPath, string outputPath, IProgress<double> progress)
        {
            string modifiedTitle = RemoveInvalidChar(videoTitle);

            var inputFile = new MediaFile
            {
                Filename = temporaryPath
            };
            var outputFile = new MediaFile
            {
                Filename = $"{outputPath}\\{modifiedTitle}.mp3"
            };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                await Task.Run(() => engine.Convert(inputFile, outputFile));
                engine.ConvertProgressEvent += (sender, e) => { progress.Report(1.0 * e.TotalDuration.TotalSeconds / e.ProcessedDuration.TotalSeconds); } ;
            }
            Dialog.DialogMessage("Finish Downloading :^)", "Download Complete");
        }

        private static string RemoveInvalidChar(string fileName)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(invalidChar, '_');
            }
            return fileName;
        }
    }
}
