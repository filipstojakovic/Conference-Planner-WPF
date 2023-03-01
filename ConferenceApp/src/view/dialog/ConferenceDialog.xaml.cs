using System.Windows;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.dialog
{
	public partial class ConferenceDialog : Window
	{
		public User UserDialog { get; set; }
		public bool Edit { get; }

		public ConferenceDialog(User user = null, bool edit = false)
		{
			Edit = edit;
			this.UserDialog = new User(user);
			InitializeComponent();
			DataContext = this;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			Close();
		}

		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
			Close();
		}
	}
}
