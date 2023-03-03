using System;
using System.Windows;
using System.Windows.Controls;

namespace ConferenceApp.view.login
{
	/// <summary>
	/// Interaction logic for RegistrationUserControl.xaml
	/// </summary>
	public partial class RegistrationUserControl : UserControl
	{
		private Action action;

		public RegistrationUserControl(Action action)
		{
			InitializeComponent();
			this.action = action;
		}

		private void Register_Click(object sender, RoutedEventArgs e)
		{
			var firstName = this.txtFirstName.Text.Trim();
			var password = this.txtPassword.Password.Trim();

			//TODO: show success/fail register message
			action();
		}
	}
}
