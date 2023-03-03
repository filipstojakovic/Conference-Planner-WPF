using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ConferenceApp.model;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.dialog
{
    public partial class UserDialog : Window
    {
        private readonly RoleDao roleDao;
        public User UserDialogData { get; set; }
        public Role SelectedRole { get; set; }
        public bool Edit { get; set; }

        public List<Role> AllRoles { get; set; }

        public UserDialog(User user = null, bool edit = true)
        {
            InitializeComponent();
            Edit = edit;
            this.UserDialogData = new User(user);
            Button.Content = edit ? "Edit" : "Create";
            this.Title = (edit ? "Edit" : "Create") + " user";

            Role userRole = UserDialogData.Roles
                .Where(role => role.Name.ToLower() == "user")
                .FirstOrDefault();
            SelectedRole = UserDialogData.Roles
                .Where(role => role.Name.ToLower() != "user")
                .DefaultIfEmpty(userRole)
                .FirstOrDefault();

            roleDao = new RoleDao();
            AllRoles = roleDao.findAll();
            DataContext = UserDialogData;
            RoleComboBox.DataContext = this;
            RoleComboBox.ItemsSource = AllRoles;

            // editBtn.Content = edit ? "Edit" : "Create";
            // editBtn.DataContext = this;
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserDialogData.Roles = new BindingList<Role> { SelectedRole };
            if (SelectedRole.Name.ToLower() != "user")
            {
                UserDialogData.Roles.Add(roleDao.findByName(UserRoleEnum.user.ToString()));
            }

            DialogResult = true;
            Close();
        }
    }
}