using System;
using System.Linq;
using System.Windows;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using Haley.Utils;

namespace ConferenceApp.utils;

// Singleton
public class AppSettings
{
	public SettingsEntity SettingsEntity { get; set; }
	public SettingsDao SettingsDao { get; private set; }

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
		this.SettingsDao = new SettingsDao();
	}

	public void applySettings()
	{
		this.applyTheme();
		this.applyLanguage();
	}

	public void changeLang(string lang)
	{
		appSettings.SettingsEntity.Language = lang;
		LangUtils.ChangeCulture(lang);
		SettingsDao.updateSettings(appSettings.SettingsEntity);
		
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
		appSettings.SettingsEntity.Theme = theme;
		SettingsDao.updateSettings(appSettings.SettingsEntity);
	}


	public void applyTheme()
	{
		this.changeTheme(SettingsEntity.Theme);
	}

	public void applyLanguage()
	{
		changeLang(SettingsEntity.Language);
	}

}