using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace YTMusicDL
{
    public class Dialog
    {
        public static void DialogMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void PromptMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
        }

        public static void ErrorMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OKCancel, MessageBoxImage.Error);
        }
    }
}
