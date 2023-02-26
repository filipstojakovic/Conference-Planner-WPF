using Haley.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace ConferenceApp.utils;

// Singleton
public class AppSettings
{
    public static readonly string SETTINGS_PATH = "settings.json";
    public static readonly string THEME = "theme";
    public static readonly string LANG = "lang";

    private static AppSettings appSettings;

    public Dictionary<string, string> settingsDictionary;
    public string path { get; }

    public static AppSettings getInstance()
    {
        if (appSettings == null)
        {
            appSettings = new AppSettings();
            appSettings.loadSettings();
        }

        return appSettings;
    }

    private AppSettings()
    {
        path = SETTINGS_PATH;
        makeDefaultSettings();
    }

    private void makeDefaultSettings()
    {
        if (File.Exists(path))
            return;

        settingsDictionary = new Dictionary<string, string>
        {
            { THEME, "default" },
            { LANG, "en" }
        };
        saveSettings();
    }

    //override [] operator
    public string this[string key]
    {
        get => settingsDictionary[key];
        set => settingsDictionary[key] = value;
    }

    public void saveSettings()
    {
        File.WriteAllText(path, JsonSerializer.Serialize(settingsDictionary));
    }

    public void loadSettings()
    {
        settingsDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path));
    }

    public void applyTheme()
    {
        changeTheme(this[THEME]);
    }

    public void changeTheme(string theme)
    {
        appSettings[THEME] = theme;
        var themeUri = new Uri("pack://application:,,,/resources/styles/" + appSettings[THEME] + ".xaml",
            UriKind.Absolute);
        var styleDictionary = Application.Current
            .Resources
            .MergedDictionaries
            .FirstOrDefault(resourceDictionary => resourceDictionary.Source.OriginalString.Contains("resources/styles"));

        styleDictionary.Source = themeUri;
    }

    public void applyLang()
    {
        changeLang(this[LANG]);
    }

    public void changeLang(string lang)
    {
        appSettings[LANG] = lang;
        LangUtils.ChangeCulture(lang);
    }
}