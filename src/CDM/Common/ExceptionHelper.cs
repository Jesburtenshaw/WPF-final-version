using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CDM.Common
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// This method return exception string
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetEndMessage(this Exception ex)
        {
            return null == ex.InnerException ? ex.Message : ex.InnerException.Message;
        }
        /// <summary>
        /// This method display dialog box with error message
        /// </summary>
        /// <param name="message"></param>
        public static void ShowErrorMessage(string message)
        {
            MessageBox.Show($"Error Occured! {Environment.NewLine}{Environment.NewLine} {message}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        /// <summary>
        /// This method display dialog box with error message
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="desc"></param>
        /// <param name="caption"></param>
        public static void ShowErrorMessage(Exception ex, string desc = "Error Occured", string caption = "Error")
        {
            MessageBox.Show($"{desc} {Environment.NewLine}{Environment.NewLine} {ex.GetEndMessage()}",
                caption,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
