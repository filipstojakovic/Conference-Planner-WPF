using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;

namespace ConferenceApp.view.login
{
    public partial class RegistrationUserControl : UserControl
    {
        private Action action;
        private UserDao userDao;
        private List<User> users;

        public RegistrationUserControl(Action action)
        {
            InitializeComponent();
            userDao = new UserDao();
            users = userDao.findAll();
            this.action = action;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var firstName = txtFirstName.Text.Trim();
            var lastName = txtLastName.Text.Trim();
            var email = txtEmail.Text.Trim();
            var password = txtPassword.Password;
            var rePassword = txtRePassword.Password;

            if (isAnyFieldEmpty())
            {
                if (areFieldsValid(email, password, rePassword))
                {
                    User user = new User
                        { FirstName = firstName, LastName = lastName, Email = email, Password = password };
                    //TODO: insert user
                    
                }
            }
            else
            {
                MessageBox.Show("Please insert all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool areFieldsValid(string email, string password, string rePassword)
        {
            if (Utils.IsValidEmailAddress(email))
            {
                MessageBox.Show("Email not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (isEmailFieldTaken(email))
            {
                MessageBox.Show("Email already in use", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (password != rePassword)
            {
                MessageBox.Show("Passwords do not match", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private bool isEmailFieldTaken(string value)
        {
            return users.Any(user => user.Email == value);
        }

        private bool isUsernameFieldTaken(string value)
        {
            return users.Any(user => user.Username == value);
        }

        private bool isAnyFieldEmpty()
        {
            var firstName = txtFirstName.Text.Trim();
            var lastName = txtLastName.Text.Trim();
            var email = txtEmail.Text.Trim();
            var password = txtPassword.Password;
            var rePassword = txtRePassword.Password;

            return firstName != "" && lastName != "" && email != "" && password != ""
                   && rePassword != "";
        }
    }
}