using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDM.Helper
{
    public static class JsonHelper
    {
        private static string drivesConfigFilePath = AppDomain.CurrentDomain.BaseDirectory + "drives.config";

        public static string GetMostRecentValue()
        {
            if (File.Exists(drivesConfigFilePath))
            {
                try
                {
                    string json = File.ReadAllText(drivesConfigFilePath);

                    var jsonObj = JsonConvert.DeserializeObject<dynamic>(json);

                    // Check if MostRecent exists and return its value
                    if (jsonObj["MostRecent"] != null)
                    {
                        return jsonObj["MostRecent"].ToString();
                    }
                    else
                    {
                        return "System";
                    }
                }
                catch (Exception ex)
                {
                    return "System";
                }
            }
            else
            {
                return "System";
            }
        }

        public static string[] PopulateArrayFromConfig()
        {
            try
            {

                if (File.Exists(drivesConfigFilePath))
                {
                    string jsonText = File.ReadAllText(drivesConfigFilePath);

                    dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(jsonText);

                    var driveArrayToken = jsonObj["Drives"];

                    if (driveArrayToken == null)
                    {
                        return null;
                    }

                    string[] driveArray = driveArrayToken.ToObject<string[]>();

                    return driveArray;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
