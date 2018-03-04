using BasicCommand;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace YTMusicDL
{
    public class YoutubeMusicViewModel : BaseViewModel
    {
        #region Private Properties
        /// <summary>
        /// Location of the directory for the Output
        /// </summary>
        private string _directoryLocation;
        #endregion

        #region Public Properties
        /// <summary>
        /// URL of the youtube video
        /// </summary>
        public string VideoURL { get; set; }

        /// <summary>
        /// Progress of the current task
        /// </summary>
        public double CurrentProgress { get; set; } = 0;

        /// <summary>
        /// Percentage of the current task
        /// </summary>
        public double ProgressPercentage { get; set; }

        /// <summary>
        /// Status indicator for the download
        /// </summary>
        public string StatusIndication { get; set; }

        /// <summary>
        /// Checks if download is currently running
        /// </summary>
        public bool IsDownloading { get; set; } = false;

        /// <summary>
        /// Location of the directory for the Output
        /// </summary>
        public string DirectoryLocation
        {
            get
            {
                return _directoryLocation;
            }
            set
            {
                if (Directory.Exists(value))
                    _directoryLocation = value;
            }
        }

        #endregion

        #region Private Variables

        /// <summary>
        /// Instance of the youtubeDownloader
        /// </summary>
        private YoutubeDownloader youtubeDownloader;
        #endregion

        #region Private Commands

        /// <summary>
        /// Browse path command
        /// </summary>
        private ICommand _browseCommand;

        /// <summary>
        /// Download Command
        /// </summary>
        private ICommand _downloadCommand;

        /// <summary>
        /// Open the containing folder
        /// </summary>
        private ICommand _openOutputFolderCommand;

        /// <summary>
        /// Opens the feedback page
        /// </summary>
        private ICommand _sendFeedbackCommand;
        #endregion

        #region Public Commands

        /// <summary>
        /// Opens the feedback page
        /// </summary>
        public ICommand SendFeedbackCommand
        {
            get { return _sendFeedbackCommand ?? (_sendFeedbackCommand = new RelayCommand(param => OpenFeedbackWindow())); }
        }

        /// <summary>
        /// Browse path command
        /// </summary>
        public ICommand BrowseCommand
        {
            get { return _browseCommand ?? (_browseCommand = new RelayCommand(param => BrowseFile())); }
        }

        /// <summary>
        /// Download Command
        /// </summary>
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                {
                    youtubeDownloader.Link = VideoURL;
                    youtubeDownloader.OutputDirectory = DirectoryLocation;
                    _downloadCommand = new RelayCommand(
                            param => DownloadMusic(),
                            param => youtubeDownloader.LinkVerification(VideoURL)
                        );
                }
                return _downloadCommand;
            }
        }

        /// <summary>
        /// Open the containing folder
        /// </summary>
        public ICommand OpenOutputFolderCommand
        {
            get { return _openOutputFolderCommand ?? (_openOutputFolderCommand = new RelayCommand(param => OpenOutputFolder())); }
        }
        
        #endregion

        #region Helper Methods

        /// <summary>
        /// Download the music and preps the progress
        /// </summary>
        private void DownloadMusic()
        {
            IsDownloading = true;
            Progress<double> progress = new Progress<double>();
            progress.ProgressChanged += Progress_ProgressChanged; 
            youtubeDownloader.DownloadMusic(progress);
        }

        /// <summary>
        /// Progress bar
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Value of the progress</param>
        private void Progress_ProgressChanged(object sender, double e)
        {
            StatusIndication = youtubeDownloader.Status;
            CurrentProgress = e;
            ProgressPercentage = e * 100;
            OnPropertyChanged("ProgressPercentage");
            OnPropertyChanged("CurrentProgress");
            OnPropertyChanged("StatusIndication");
        }

        /// <summary>
        /// Browse path for the output
        /// </summary>
        private void BrowseFile()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var result = dialog.ShowDialog();
                DirectoryLocation = dialog.SelectedPath;
                ConfigCommands.SaveLocation(DirectoryLocation);
                OnPropertyChanged("DirectoryLocation");
            }
        }

        /// <summary>
        /// Open Output folder
        /// </summary>
        private void OpenOutputFolder()
        {
            try
            {
                Process.Start(DirectoryLocation);
            }
            catch (InvalidOperationException)
            {
                Dialog.ErrorMessage("No file location was specified", "Error");
            }
        }

        /// <summary>
        /// Open the feedback window
        /// </summary>
        private void OpenFeedbackWindow()
        {
            Feedback feedback = new Feedback();
            feedback.ShowDialog();
        }

        #endregion

        #region Constructor
        public YoutubeMusicViewModel()
        {
            youtubeDownloader = new YoutubeDownloader();
            var location = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            DirectoryLocation = ConfigCommands.LoadSavedLocation(location.Substring(0, location.LastIndexOf('\\') + 1) + "Downloads");
        }
        #endregion
    }
}
