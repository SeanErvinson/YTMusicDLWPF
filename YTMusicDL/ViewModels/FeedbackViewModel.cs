using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace YTMusicDL
{
    class FeedbackViewModel : BaseViewModel
    {
        #region Public Properties
        public string Subject { get; set; }
        public string Message { get; set; }
        #endregion

        #region Private Commands
        private ICommand _sendFeedbackCommand;

        #endregion

        #region Public Commands
        public ICommand SendFeedbackCommand
        {
            get { return _sendFeedbackCommand; }
            set { _sendFeedbackCommand = value; }
        }
        #endregion

        #region Helper Methods

        #endregion
    }
}
