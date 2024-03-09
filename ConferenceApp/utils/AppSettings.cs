using Haley.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using ConferenceApp.model.entity;
using Google.Protobuf;

namespace ConferenceApp.utils;

// Singleton
public class AppSettings
{
    public SettingsEntity SettingsEntity { get; set; }

    private static AppSettings appSettings;

    public static AppSettings getInstance()
    {
        if (appSettings == null)
        {
            appSettings = new AppSettings();
        }

        return appSettings;
    }

    private AppSettings()
    {
        SettingsEntity = new SettingsEntity();
    }

    public void changeLang(string lang)
    {
        LangUtils.ChangeCulture(lang);
    }

    public void changeTheme(string theme)
    {
        var themeUri = new Uri("pack://application:,,,/resources/styles/" + theme + ".xaml",
            UriKind.Absolute);
        var styleDictionary = Application.Current
            .Resources
            .MergedDictionaries
            .FirstOrDefault(resourceDictionary =>
                resourceDictionary.Source.OriginalString.Contains("resources/styles"));

        styleDictionary.Source = themeUri;
    }


    public void applyTheme()
    {
        this.changeTheme(SettingsEntity.Theme);
    }

    public void applyLanguage()
    {
        changeLang(SettingsEntity.Language);
    }

    public void applySettings()
    {
        this.applyTheme();
        this.applyLanguage();
    }
}