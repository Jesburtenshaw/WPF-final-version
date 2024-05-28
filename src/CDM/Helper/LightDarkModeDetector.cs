using Microsoft.Win32.SafeHandles;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace CDM.Helper
{
    internal class LightDarkModeDetector
    {
        #region :: Constructor ::
        public LightDarkModeDetector() { }
        #endregion
        #region :: Variables ::
        int res = (int)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", -1);
        #endregion
    }
}
