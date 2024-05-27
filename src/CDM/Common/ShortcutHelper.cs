using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDM.Common
{
    public static class ShortcutHelper
    {
        public static string GetLnkTarget(string lnkPath)
        {
            try
            {
                // Create a Shell object
                dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("Shell.Application"));

                // Get the folder containing the .lnk file
                var folder = shell.NameSpace(System.IO.Path.GetDirectoryName(lnkPath));

                // Get the .lnk file
                var folderItem = folder.ParseName(System.IO.Path.GetFileName(lnkPath));

                // Get the target of the .lnk file
                dynamic link = folderItem.GetLink;

                return link.Path;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool CreateLnk(string lnkPath, string targetPath)
        {
            try
            {
                // Create a Shell object
                dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("WScript.Shell"));
                dynamic shortcut = shell.CreateShortcut(lnkPath);
                shortcut.TargetPath = targetPath;
                shortcut.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
