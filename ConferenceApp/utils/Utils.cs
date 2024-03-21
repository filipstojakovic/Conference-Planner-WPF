using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using MySql.Data.MySqlClient;

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

        public static bool DateRangesOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            if (start1 <= end2 && end1 >= start2)
            {
                return true;
            }

            return false;
        }

        public static DateTime combineDateAndTime(DateTime date , DateTime time )
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }

        public static bool isColumnNull(MySqlDataReader reader, string columnName)
        {
            var columnOrdinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(columnOrdinal);
        }

        public static T readerGetValue<T>(MySqlDataReader reader, string columnName)

        {
            int columnIndex = reader.GetOrdinal(columnName); // Get the index of the column by name

            if (reader.IsDBNull(columnIndex)) // Check if the value is null
            {
                return default(T);
            }
            else
            {
                object value = reader.GetValue(columnIndex); // Get the value of the column as an object
                return (T)value;
            }
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