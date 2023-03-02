using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
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
            HeaderUserText.Text = Utils.CapitalizeFirstLetter(user.FirstName) + " " +
                                  Utils.CapitalizeFirstLetter(user.LastName);
        }

        public MainWindow() // TODO: make private, check if login user isAdmin or !isAdmin
        {
            Trace.WriteLine("before init MainWindow");
            InitializeComponent();
            Trace.WriteLine("after init MainWindow");

            // Drawer items
            Drawer_MenuListView.Items.Add(new ItemMenu("Conferences", PackIconKind.Register, new ConferenceControl()));
            Drawer_MenuListView.Items.Add(new ItemMenu("Sessions", PackIconKind.Connection, new UsersControl()));
            Drawer_MenuListView.Items.Add(new ItemMenu("Events", PackIconKind.Connection, new UsersControl()));
            Drawer_MenuListView.Items.Add(new ItemMenu("Menu Control", PackIconKind.About, new MenuControl()));
            Drawer_MenuListView.Items.Add(new ItemMenu("Users", PackIconKind.Connection, new UsersControl()));
            Drawer_MenuListView.SelectedIndex = 0; // select first menu item by default


            DrawerBottom_MenuListView.Items.Add(new ItemMenu("Settings", PackIconKind.Settings, new SettingsControl()));
        }

        //Selection changed in left menu
        private void Drawer_MenuListView_MouseLeftButtonUp(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = ((ListView)sender).SelectedItem;
            SwitchScreen((ItemMenu)selectedItem);
            DrawerBottom_MenuListView.SelectedItems.Clear();
        }

        private void DrawerBottom_MenuListView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
            Drawer_MenuListView.SelectedItems.Clear();
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