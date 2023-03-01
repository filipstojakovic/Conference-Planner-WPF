using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConferenceApp.model.entity;
using ConferenceApp.view.dialog;

namespace GenSpark.WPFAccordionMenu
{
	public partial class MenuControl : UserControl
	{
		private BindingList<User> userList;

		public MenuControl()
		{
			userList = new BindingList<User>() { new User { FirstName = "Filip", Roles = new BindingList<Role>() { new Role { Name = "role" }, new Role { Name = "role1" } } }, new User { FirstName = "nebojsa" } };
			InitializeComponent();
			itemsControl.ItemsSource = userList;
			userList.ListChanged += UserList_ListChanged;
		}

		private void UserList_ListChanged(object sender, ListChangedEventArgs e)
		{
			Console.WriteLine();
		}

		private void User_Button_MouseLeftButtonUp(object sender, RoutedEventArgs e)
		{
			Random random = new Random();
			userList.Add(new User { FirstName = "test " + random.Next(100100100) });
		}
		private void Role_Button_MouseLeftButtonUp(object sender, RoutedEventArgs e)
		{
			User user = new User
			{
				FirstName = "filip",
				LastName = "stojak"
			};
			var dialog = new ConferenceDialog(user);
			if (dialog.ShowDialog() == true)
			{
				MessageBox.Show("You said: " + dialog.UserDialog);
			}
			else
			{
				var test = dialog.UserDialog;
			}
			//Random random = new Random();
			//userList[0].Roles.Add(new Role { Name = "newRole " + random.Next(100100100) });
		}

		private void itemsControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{

			Console.WriteLine();
		}
	}
}
