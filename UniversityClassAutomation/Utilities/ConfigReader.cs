using System.IO;
using Newtonsoft.Json.Linq;

namespace UniversityClassAutomation.Utilities
{
    public static class ConfigReader
    {
        private static readonly JObject _config;

        static ConfigReader()
        {
            var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            _config = JObject.Parse(File.ReadAllText(configPath));
        }

        public static string GetValue(string section, string key)
        {
            return _config[section]?[key]?.ToString();
        }
    }
}
