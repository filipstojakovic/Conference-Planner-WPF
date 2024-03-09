using System.Windows;
using ConferenceApp.utils;
using Haley.Utils;

namespace ConferenceApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			LangUtils.Register();
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			AppSettings.getInstance().applyTheme();
		}

		// private void App_OnExit(object sender, ExitEventArgs e)
		// {
		// 	AppSettings.getInstance().saveSettings();
		// }
	}
}
