using System;
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
using MySql.Data.MySqlClient;

namespace ConferenceApp.view
{
    public partial class MainWindow : Window
    {
        private User currentUser;

        public MainWindow(User currentUser)
        {
            Trace.WriteLine("before init MainWindow");
            InitializeComponent();
            Trace.WriteLine("after init MainWindow");
            this.currentUser = currentUser;
            HeaderUserText.Text = Utils.CapitalizeFirstLetter(currentUser.FirstName) + " " +
                                  Utils.CapitalizeFirstLetter(currentUser.LastName);

            Drawer_MenuListView.Items.Add(new ItemMenu("Conferences", PackIconKind.Register,
                new ConferenceControl(this.currentUser)));
            Drawer_MenuListView.Items.Add(new ItemMenu("Sessions", PackIconKind.Connection,
                new SessionControl(this.currentUser)));
            Drawer_MenuListView.Items.Add(new ItemMenu("Events", PackIconKind.Connection,
                new EventControl(this.currentUser)));

            //TODO: viska, ali necemu ce sluziti
            Drawer_MenuListView.Items.Add(new ItemMenu("Menu Control", PackIconKind.About, new MenuControl()));

            //if user has admin role
            if (currentUser.Roles.Any(role => role.Name.ToLower() == UserRoleEnum.admin.ToString()))
            {
                Drawer_MenuListView.Items.Add(new ItemMenu("Locations", PackIconKind.Map,
                    new LocationControl()));
                Drawer_MenuListView.Items.Add(new ItemMenu("Users", PackIconKind.User,
                    new UsersControl(this.currentUser)));
            }

            Drawer_MenuListView.SelectedIndex = 0; // select first menu item by default
            DrawerBottom_MenuListView.Items.Add(new ItemMenu("Settings", PackIconKind.Settings, new SettingsControl()));


            
          // test();
          // ConferenceDao conferenceDao = new ConferenceDao();
          // conferenceDao.deleteConference(2);
        }

        void test()
        {
            Conference conference = Generate.conference();
            UserGatheringRoleDao userGatheringRoleDao = new UserGatheringRoleDao();
            User user = (new UserDao()).findById(2);
            userGatheringRoleDao.insertUserConferenceConferenceRole(this.currentUser, conference,
                GatheringRoleEnum.Organizer);
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

        private void SwitchScreen(ItemMenu sender)
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