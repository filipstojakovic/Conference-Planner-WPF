using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.view.usercontrol.ViewModel;
using MaterialDesignThemes.Wpf;
using ConferenceApp.view.usercontrol;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConferenceApp.view.login;

namespace ConferenceApp.view
{
	public partial class MainWindow : Window
    {
        public MainWindow() // Need to get user to know isAdmin or !
        {
            InitializeComponent();

            UserDao userDao = new UserDao();
            User user = userDao.getUserById(11);

            Console.WriteLine();

            var userControl = new ItemMenu("Register", PackIconKind.Register, new UserControlCustomers());
            var emplyeControl = new ItemMenu("Employees", PackIconKind.Connection, new UserControlProviders());

            LeftMenuListView.Items.Add(emplyeControl);
            LeftMenuListView.Items.Add(userControl);
            LeftMenuListView.Items.Add(emplyeControl);
            LeftMenuListView.Items.Add(userControl);
            LeftMenuListView.Items.Add(emplyeControl);
            LeftMenuListView.Items.Add(userControl);

            HeaderMenuListView.Items.Add(new ItemMenu("Settings",PackIconKind.Settings,new SettingsControl()));
            HeaderMenuListView.Items.Add(userControl);
            HeaderMenuListView.Items.Add(emplyeControl);
            HeaderMenuListView.Items.Add(userControl);

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
