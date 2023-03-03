using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConferenceApp.model;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using ConferenceApp.view.login;
using ConferenceApp.view.usercontrol;
using GenSpark.WPFAccordionMenu;
using MaterialDesignThemes.Wpf;

namespace ConferenceApp.view
{
    public partial class MainWindow : Window
    {
        private User currentUser;

        public MainWindow(User user)
        {
            Trace.WriteLine("before init MainWindow");
            InitializeComponent();
            Trace.WriteLine("after init MainWindow");
            this.currentUser = user;
            HeaderUserText.Text = Utils.CapitalizeFirstLetter(user.FirstName) + " " +
                                  Utils.CapitalizeFirstLetter(user.LastName);

            Drawer_MenuListView.Items.Add(new ItemMenu("Conferences", PackIconKind.Register, 
                new ConferenceControl(currentUser)));
            Drawer_MenuListView.Items.Add(new ItemMenu("Sessions", PackIconKind.Connection,
                new SessionControl(currentUser)));
            Drawer_MenuListView.Items.Add(new ItemMenu("Events", PackIconKind.Connection,
                new EventControl(currentUser)));
            
            //TODO: viska, ali necemu ce sluziti
            Drawer_MenuListView.Items.Add(new ItemMenu("Menu Control", PackIconKind.About, new MenuControl()));

            //if user has admin role
            if (user.Roles.Any(role => role.Name.ToLower() == UserRoleEnum.admin.ToString()))
            {
                Drawer_MenuListView.Items.Add(new ItemMenu("Locations", PackIconKind.Map,
                    new LocationControl()));
                Drawer_MenuListView.Items.Add(new ItemMenu("Users", PackIconKind.User,
                    new UsersControl(currentUser)));
            }

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