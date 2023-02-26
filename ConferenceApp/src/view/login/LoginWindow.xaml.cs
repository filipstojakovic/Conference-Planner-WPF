using System.Windows;
using System.Windows.Controls;
using ConferenceApp.model.dao;
using ConferenceApp.utils;

namespace ConferenceApp.view.login;

public partial class LoginWindow : Window
{
	private AppSettings appSettings;
	private UserDao userDao;

	public LoginWindow()
	{
		appSettings = AppSettings.getInstance();
		var lang = appSettings[AppSettings.LANG];

		InitializeComponent();

		DockPanelMain.Children.Clear();
		DockPanelMain.Children.Add(new LoginUserControl());

		var frame = Application.Current;
	}

	private void Language_ToggleButton_OnChecked(object sender, RoutedEventArgs e)
	{
		var radioButton = sender as RadioButton;
		if (radioButton == null)
			return;

		appSettings.changeLang("en" == radioButton.Name ? "en" : "sr");
	}

	private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
	{
		var radioButton = sender as RadioButton;
		if (radioButton == null || DockPanelMain == null)
			return;

		DockPanelMain.Children.Clear();
		if ("login" == radioButton.Name)
		{
			DockPanelMain.Children.Add(new LoginUserControl());
		}
		else
		{
			DockPanelMain.Children.Add(new RegistrationUserControl());

		}
	}
}