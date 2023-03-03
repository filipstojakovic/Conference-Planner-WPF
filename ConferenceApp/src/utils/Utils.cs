using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ConferenceApp.utils
{
    class Utils
    {
        public const string EmailRegexPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

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
        
        public static bool IsValidEmailAddress(string s)
        {
            Regex regex = new Regex(EmailRegexPattern);
            return regex.IsMatch(s);
        }
    }
}