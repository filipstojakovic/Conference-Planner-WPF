using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ConferenceApp.utils
{
    class Utils
    {
        public static void createFileIfNotExists(string path)
        {
            if (File.Exists(path))
                return;
            File.Create(path);
        }

        public static Dictionary<string, string> readJson(String path)
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path));
        }

        public static string CapitalizeFirstLetter(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
    }
}