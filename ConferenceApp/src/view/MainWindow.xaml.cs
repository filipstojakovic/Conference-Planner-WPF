using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.view.login;
using ConferenceApp.view.usercontrol;
using ConferenceApp.view.usercontrol.ViewModel;
using GenSpark.WPFAccordionMenu;
using MaterialDesignThemes.Wpf;

namespace ConferenceApp.view
{
	public partial class MainWindow : Window
	{
		private User logedInUser;

		public MainWindow(User user) : this()
		{
			this.logedInUser = user;
		}

		public MainWindow() // TODO: check if login user isAdmin or !isAdmin
		{
			InitializeComponent();

			UserDao userDao = new UserDao();
			//User user = userDao.getUserByUsername("admin");
			List<User> users = userDao.getAllUsers();

			RoleDao roleDao = new RoleDao();

			//TODO: TESTING

			var userControl = new ItemMenu("Register", PackIconKind.Register, new UserControlCustomers());
			var emplyeControl = new ItemMenu("Employees", PackIconKind.Connection, new UsersControl());
			var menuControl = new ItemMenu("Menu Control", PackIconKind.About, new MenuControl());


			// Drawer items
			LeftMenuListView.Items.Add(emplyeControl);
			LeftMenuListView.Items.Add(userControl);
			LeftMenuListView.Items.Add(menuControl);


			// Header items
			HeaderMenuListView.Items.Add(new ItemMenu("Settings", PackIconKind.Settings, new SettingsControl()));
			HeaderMenuListView.Items.Add(userControl);
			HeaderMenuListView.Items.Add(emplyeControl);
			//TODO: add logout button

			LeftMenuListView.SelectedIndex = 0; // select first menu item by default
		}

		//Selection changed in left menu
		private void LeftMenuListView_MouseLeftButtonUp(object sender, SelectionChangedEventArgs e)
		{
			var selectedItem = ((ListView)sender).SelectedItem;
			SwitchScreen((ItemMenu)selectedItem);
			HeaderMenuListView.SelectedItems.Clear();
		}

		private void HeaderMenuListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			var selectedItem = ((ListView)sender).SelectedItem;
			var itemMenu = selectedItem as ItemMenu;
			if (itemMenu == null)
			{
				new LoginWindow().Show();
				this.Close();
				return;
			}
			SwitchScreen(itemMenu);
			LeftMenuListView.SelectedItems.Clear();
		}

		internal void SwitchScreen(ItemMenu sender)
		{
			if (sender == null || sender.Screen == null)
				return;

			var screen = sender.Screen;
			DockPanelMain.Children.Clear();
			DockPanelMain.Children.Add(screen);
			HeaderBar.Text = sender.Header;
		}
	}
}
