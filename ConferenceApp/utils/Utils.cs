using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;

namespace ConferenceApp.utils
{
    class Utils
    {
        public const string EmailRegexPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,5}$";

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

        public static void ErrorBox(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void InfoBox(string message)
        {
            MessageBox.Show(message, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static bool confirmAction(string message)
        {
            MessageBoxResult result =
                MessageBox.Show(message, "Confirmation",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

            return result == MessageBoxResult.Yes;
        }
    }
}