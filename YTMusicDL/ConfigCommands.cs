using System;
using Newtonsoft.Json;
using System.IO;

namespace BasicCommand
{
    public class ConfigCommands
    {
        private static string ConfigFileName { get; set; } = "config.json";
        private static string ConfigFileLocation { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),ConfigFileName);

        public static void SaveLocation(string directory)
        {
            if (!File.Exists(ConfigFileLocation))
            {
                var file = File.Create(ConfigFileLocation);
                file.Close();
            }

            var config = new Config()
            {
                DefaultDirectory = directory
            };

            string json = JsonConvert.SerializeObject(config);

            using (var writer = new StreamWriter(ConfigFileLocation, false))
            {
                writer.Write(json);
            }

        }

        public static string LoadSavedLocation(string defaultLocation = null)
        {
            if (File.Exists(ConfigFileLocation))
            {
                using (var r = new StreamReader(ConfigFileLocation))
                {
                    string json = r.ReadToEnd();
                    Config location = JsonConvert.DeserializeObject<Config>(json);
                    return location.DefaultDirectory;
                }
            }
            return defaultLocation;
        }
    }
}
