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
        public LightDarkModeDetector() { }

        int res = (int)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", -1);

        //private static bool IsLightTheme()
        //{
        //    var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
        //    var value = key?.GetValue("AppsUseLightTheme");
        //    return value is int i && i > 0;
        //}

        //protected override void OnSourceInitialized(EventArgs e)
        //{
        //    base.OnSourceInitialized(e);

        //    // Detect when the theme changed
        //    HwndSource source = (HwndSource)PresentationSource.FromVisual(this);
        //    source.AddHook((IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) =>
        //    {
        //        const int WM_SETTINGCHANGE = 0x001A;
        //        if (msg == WM_SETTINGCHANGE)
        //        {
        //            if (wParam == IntPtr.Zero && Marshal.PtrToStringUni(lParam) == "ImmersiveColorSet")
        //            {
        //                var isLightTheme = IsLightTheme();
        //                // TODO Change app theme accordingly
        //            }
        //        }

        //        return IntPtr.Zero;
        //    });
        //}


        //public static bool SystemUsesLightTheme
        //{
        //    get
        //    {
        //        bool systemUsesLightTheme = GetDWORDBoolValue(ColorMode_RegKey, ColorMode_SystemUsesLightTheme, true);
        //        return systemUsesLightTheme;
        //    }
        //    set
        //    {
        //        if (value != SystemUsesLightTheme)
        //        {
        //            SetDWORDBoolValue(ColorMode_RegKey, ColorMode_SystemUsesLightTheme, value);
        //        }
        //    }
        //}

        //const string ColorMode_RegKey = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        //const string ColorMode_AppsUseLightTheme = "AppsUseLightTheme";

        //public static bool GetDWORDBoolValue(string registryKey, string valueName, bool defaultValue)
        //{
        //    int defVal = defaultValue ? 1 : 0;
        //    int iVal = (int)Registry.GetValue(registryKey, valueName, defVal);

        //    var boolval = iVal == 0 ? false : true;
        //    return boolval;
        //}

        //[SecuritySafeCritical]
        //public static object GetValue(string keyName, string valueName, object defaultValue)
        //{
        //    string subKeyName;
        //    RegistryKey baseKeyFromKeyName = GetBaseKeyFromKeyName(keyName, out subKeyName);
        //    RegistryKey registryKey = baseKeyFromKeyName.OpenSubKey(subKeyName);
        //    if (registryKey == null)
        //    {
        //        return null;
        //    }

        //    try
        //    {
        //        return registryKey.GetValue(valueName, defaultValue);
        //    }
        //    finally
        //    {
        //        registryKey.Close();
        //    }
        //}


        //[SecurityCritical]
        //private static RegistryKey GetBaseKeyFromKeyName(string keyName, out string subKeyName)
        //{
        //    if (keyName == null)
        //    {
        //        throw new ArgumentNullException("keyName");
        //    }

        //    int num = keyName.IndexOf('\\');
        //    string text = ((num == -1) ? keyName.ToUpper(CultureInfo.InvariantCulture) : keyName.Substring(0, num).ToUpper(CultureInfo.InvariantCulture));
        //    RegistryKey registryKey = null;
        //    //registryKey = text switch
        //    //{
        //    //    "HKEY_CURRENT_USER" => CurrentUser,
        //    //    "HKEY_LOCAL_MACHINE" => LocalMachine,
        //    //    "HKEY_CLASSES_ROOT" => ClassesRoot,
        //    //    "HKEY_USERS" => Users,
        //    //    "HKEY_PERFORMANCE_DATA" => PerformanceData,
        //    //    "HKEY_CURRENT_CONFIG" => CurrentConfig,
        //    //    "HKEY_DYN_DATA" => RegistryKey.GetBaseKey(RegistryKey.HKEY_DYN_DATA),
        //    //    _ => throw new ArgumentException(Environment.GetResourceString("Arg_RegInvalidKeyName", "keyName")),
        //    //};
        //    if (num == -1 || num == keyName.Length)
        //    {
        //        subKeyName = string.Empty;
        //    }
        //    else
        //    {
        //        subKeyName = keyName.Substring(num + 1, keyName.Length - num - 1);
        //    }

        //    return registryKey;
        //}

        //[SecuritySafeCritical]
        //public RegistryKey OpenSubKey(string name, bool writable)
        //{
        //    ValidateKeyName(name);
        //    EnsureNotDisposed();
        //    name = FixupName(name);
        //    CheckPermission(RegistryInternalCheck.CheckOpenSubKeyWithWritablePermission, name, writable, RegistryKeyPermissionCheck.Default);
        //    SafeRegistryHandle hkResult = null;
        //    int num = Win32Native.RegOpenKeyEx(hkey, name, 0, GetRegistryKeyAccess(writable) | (int)regView, out hkResult);
        //    if (num == 0 && !hkResult.IsInvalid)
        //    {
        //        RegistryKey registryKey = new RegistryKey(hkResult, writable, systemkey: false, remoteKey, isPerfData: false, regView);
        //        registryKey.checkMode = GetSubKeyPermissonCheck(writable);
        //        registryKey.keyName = keyName + "\\" + name;
        //        return registryKey;
        //    }

        //    if (num == 5 || num == 1346)
        //    {
        //        ThrowHelper.ThrowSecurityException(ExceptionResource.Security_RegistryPermission);
        //    }

        //    return null;
        //}
    }
}
