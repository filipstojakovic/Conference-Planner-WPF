using System.Windows;
using System.Windows.Controls;

namespace ConferenceApp.view.login
{
	/// <summary>
	/// Interaction logic for RegistrationUserControl.xaml
	/// </summary>
	public partial class RegistrationUserControl : UserControl
	{
		public RegistrationUserControl()
		{
			InitializeComponent();
		}

		private void Register_Click(object sender, RoutedEventArgs e)
		{
			var firstName = this.txtFirstName.Text.Trim();
			var password = this.txtPassword.Password.Trim();

			//new MainWindow().Show(); 
			//this.Close();
		}
	}
}
