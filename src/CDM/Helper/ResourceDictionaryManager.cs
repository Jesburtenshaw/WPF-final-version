using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CDM.Helper
{
    internal class ResourceDictionaryManager
    {
        #region :: Methods ::
        /// <summary>
        /// This method used to Update or merge Dictionary of resources
        /// </summary>
        /// <param name="resourceLookup"></param>
        /// <param name="newResourceUri"></param>
        /// <param name="uc"></param>
        /// <returns></returns>
        public static bool UpdateDictionary(string resourceLookup, Uri newResourceUri,UserControl uc)
        {
            Collection<ResourceDictionary> applicationDictionaries = GetApplicationMergedDictionaries(uc);

            if (applicationDictionaries.Count == 0 || newResourceUri is null)
            {
                return false;
            }

            resourceLookup = resourceLookup.ToLower().Trim();

            for (var i = 0; i < applicationDictionaries.Count; i++)
            {
                string sourceUri;

                if (applicationDictionaries[i]?.Source != null)
                {
                    sourceUri = applicationDictionaries[i].Source.ToString().ToLower().Trim();

                    if (sourceUri.Contains(resourceLookup))
                    {
                        applicationDictionaries[i] = new ResourceDictionary() { Source = newResourceUri };

                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// This method merge dictionaries
        /// </summary>
        /// <param name="uc"></param>
        /// <returns></returns>
        private static Collection<ResourceDictionary> GetApplicationMergedDictionaries(UserControl uc)
        {
            return uc.Resources.MergedDictionaries;
            //return Application.Current.Resources.MergedDictionaries;
        }
        /// <summary>
        /// This method is used to update dictionary
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="value"></param>
        public static void UpdateResourceColor(string resourceName, object value)
        {
            Application.Current.Resources[resourceName] = value;
        }
        #endregion
    }
}
