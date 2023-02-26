using System.Windows;
using System.Windows.Controls;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;

namespace ConferenceApp.view.login
{
	/// <summary>
	/// Interaction logic for LoginUserControl.xaml
	/// </summary>
	public partial class LoginUserControl : UserControl
	{
		private AppSettings appSettings;
		private UserDao userDao;


		public LoginUserControl()
		{
			appSettings = AppSettings.getInstance();
			userDao = new UserDao();
			InitializeComponent();

			//if ("sr" == lang)
			//	sr.IsChecked = true;
			//else
			//	en.IsChecked = true;
		}

		private void Login_Click(object sender, RoutedEventArgs e)
		{
			var username = this.txtUsername.Text.Trim();
			var password = this.txtPassword.Password.Trim();

			//TODO: check if normal user or admin

			User user = userDao.getUserByUsername(username);
			if (checkIfUsernameAndPasswordValid(user, username, password))
			{
				new MainWindow(user).Show();
				//this.Close();
			}
			if ("a" == username && "a" == password)
			{
				new MainWindow().Show(); // TODO: send logged in user
										 //this.Close();
			}
			else
			{
				//ERROR MESSAGE, user doesnt exist
				MessageBox.Show("Wrong username or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private bool checkIfUsernameAndPasswordValid(User user, string username, string password)
		{
			if (user == null)
				return false;

			string encodedPassword = Base64Util.Base64Encode(password);
			return user.UserName == username && encodedPassword == user.Password;
		}

	}
}
