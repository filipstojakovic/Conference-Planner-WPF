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
            if(File.Exists(path))
                return;
            File.Create(path);
        }

        public static Dictionary<string, string> readJson(String path)
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path));
        }

    }
}