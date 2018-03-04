using YoutubeExplode;
using System.IO;
using YoutubeExplode.Models.MediaStreams;
using System;
using System.Threading;

namespace YTMusicDL
{
    public class YoutubeDownloader
    {
        #region Public Properties
        public string Link { get; set; }
        public string OutputDirectory { get; set; }
        public string Status { get; set; }
        #endregion

        #region Private Properties
        private string _temporaryPath { get; set; }
        private string _verifiedLink { get; set; }
        
        #endregion

        public bool LinkVerification(string link)
        {
            if(YoutubeClient.TryParseVideoId(link, out string check))
            {
                _verifiedLink = check;
                return true;
            }
            return false;
        }

        public async void DownloadMusic(IProgress<double> progress)
        {
            var client = new YoutubeClient();
            
            var video = await client.GetVideoAsync(_verifiedLink);

            var streamInfoSet = await client.GetVideoMediaStreamInfosAsync(_verifiedLink);

            var audio = streamInfoSet.Audio.WithHighestBitrate();

            _temporaryPath = Path.GetTempFileName();

            Status = "Downloading";
            CancellationToken cancellationToken = new CancellationToken();
            await client.DownloadMediaStreamAsync(audio, _temporaryPath, progress);

            Status = "Converting";
            await AudioManager.ToAudio(video.Title, _temporaryPath, OutputDirectory, progress);

            progress.Report(0);
            Status = "Complete";
        }
    }
}
