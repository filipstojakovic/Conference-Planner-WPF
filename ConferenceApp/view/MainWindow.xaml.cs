using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
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
using Haley.Utils;
using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;

namespace ConferenceApp.view
{
    public partial class MainWindow : Window
    {
        private readonly User currentUser;
        private readonly UserDao userDao;

        public MainWindow(User currentUser)
        {
            Trace.WriteLine("before init MainWindow");
            InitializeComponent();
            Trace.WriteLine("after init MainWindow");
            
            this.currentUser = currentUser;
            userDao = new UserDao();

            setUserFullNameHeader(currentUser);
            setDrawerButton(currentUser);
            
            LiveEvent liveEvent = new LiveEvent
            {
                Name = "event",
                Description = "desc",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(5),
                
            }
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

            //Used for testing
            // Drawer_Upper_MenuListView.Items.Add(new ItemMenu("Menu Control", PackIconKind.About,
            //     () => new MenuControl()));

            //if user has admin role
            if (currentUser.Roles.Any(role => role.Name.ToLower() == UserRoleEnum.admin.ToString()))
            {
                // Drawer_Upper_MenuListView.Items.Add(new ItemMenu(LangUtils.Translate("locations"), PackIconKind.Map,
                //     () => new LocationControl()));
                Drawer_Upper_MenuListView.Items.Add(new ItemMenu("users", PackIconKind.User,
                    () => new UsersControl(this.currentUser)));
            }

            Drawer_Upper_MenuListView.SelectedIndex = 0; // select first menu item by default
            Drawer_Bottom_MenuListView.Items.Add(new ItemMenu("settings", PackIconKind.Settings,
                () => new SettingsControl(Drawer_Upper_MenuListView, Drawer_Bottom_MenuListView)));
        }

        private void setUserFullNameHeader(User currentUser)
        {
            try
            {
                string userFullName = userDao.getUserFullname_Procedure(currentUser.Id);
                string[] fullName = userFullName.Split(' ');
                HeaderUserText.Text = Utils.CapitalizeFirstLetter(fullName[0]) + " " +
                                      Utils.CapitalizeFirstLetter(fullName[1]);
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