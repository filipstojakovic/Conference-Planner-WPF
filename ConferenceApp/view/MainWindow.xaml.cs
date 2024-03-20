using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using ConferenceApp.view.login;
using ConferenceApp.view.usercontrol;
using Haley.Utils;
using MaterialDesignThemes.Wpf;

namespace ConferenceApp.view
{
	public partial class MainWindow : Window
	{
		private readonly User currentUser;
		private readonly UserDao userDao;

		public MainWindow(User currentUser)
		{
			AppSettings.getInstance().applySettings();
			InitializeComponent();

			this.currentUser = currentUser;
			userDao = new UserDao();

			setUserFullNameHeader(currentUser);
			setDrawerButton(currentUser);
		}

		private void setDrawerButton(User currentUser)
		{
			Drawer_Upper_MenuListView.Items.Add(new ItemMenu("conferences", //LangUtils.Translate("conferences"),
				PackIconKind.ViewAgenda,
				() => new ConferenceControl(this.currentUser)));
			Drawer_Upper_MenuListView.Items.Add(new ItemMenu("sessions",
				PackIconKind.ViewSequential,
				() => new SessionControl(this.currentUser)));
			Drawer_Upper_MenuListView.Items.Add(new ItemMenu("events", PackIconKind.ViewHeadline,
				() => new EventControl(this.currentUser)));

			if (currentUser.isAdmin())
			{
				Drawer_Upper_MenuListView.Items.Add(new ItemMenu("users", PackIconKind.User,
					() => new UsersControl(this.currentUser)));

			}

			Drawer_Upper_MenuListView.SelectedIndex = 0; // select first menu item by default
			Drawer_Bottom_MenuListView.Items.Add(new ItemMenu("settings", PackIconKind.Settings,
				() => new SettingsControl(Drawer_Upper_MenuListView, Drawer_Bottom_MenuListView)));
			// Drawer_Bottom_MenuListView.Items.Add(new ItemMenu("about", PackIconKind.About,
			//     () => new AboutControl()));
		}

		private void setUserFullNameHeader(User currentUser)
		{
			try
			{
				string userFullName = Utils.CapitalizeFirstLetter(currentUser.FirstName) + " " +
									  Utils.CapitalizeFirstLetter(currentUser.LastName);
				HeaderUserChip.Content = userFullName;
				HeaderUserChip.Icon = currentUser.FirstName.Substring(0, 1).ToUpper();
			}
			catch (Exception)
			{
				Utils.ErrorBox("Unable to set users fullname");
			}
		}

		//Selection changed in left menu
		private void Drawer_MenuListView_MouseLeftButtonUp(object sender, SelectionChangedEventArgs e)
		{
			var selectedItem = ((ListView)sender).SelectedItem;
			if (selectedItem == null)
				return;
			SwitchScreen((ItemMenu)selectedItem);
			Drawer_Bottom_MenuListView.SelectedItems.Clear();
		}

		private void DrawerBottom_MenuListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			var selectedItem = ((ListView)sender).SelectedItem;
			var itemMenu = selectedItem as ItemMenu;
			if (itemMenu == null)
			{
				var result = Utils.confirmAction("Are you sure you want to log out?");
				if (result)
				{
					AppSettings.getInstance().SettingsEntity = new SettingsEntity();
					AppSettings.getInstance().applySettings();
					new LoginWindow().Show();
					this.Close();
				}

				Drawer_Logout_MenuListView.SelectedItems.Clear();
				return;
			}

			SwitchScreen(itemMenu);
			Drawer_Upper_MenuListView.SelectedItems.Clear();
		}


		private void SwitchScreen(ItemMenu sender)
		{
			if (sender == null || sender.CreateUserControl == null)
				return;

			var screen = sender.CreateUserControl();
			DockPanelMain.Children.Clear();
			DockPanelMain.Children.Add(screen);
			HeaderBar.Text = LangUtils.Translate(sender.Header);
		}
	}
}