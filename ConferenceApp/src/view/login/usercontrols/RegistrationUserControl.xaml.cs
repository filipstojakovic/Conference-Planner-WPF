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
		private Action closeAction;

		public RegistrationUserControl(Action closeAction)
		{
			InitializeComponent();
			this.closeAction = closeAction;
		}

		private void Register_Click(object sender, RoutedEventArgs e)
		{
			var firstName = this.txtFirstName.Text.Trim();
			var password = this.txtPassword.Password.Trim();

			new MainWindow().Show();
			closeAction();
		}
	}
}
