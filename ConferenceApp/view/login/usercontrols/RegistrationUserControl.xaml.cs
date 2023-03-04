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
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Password;
            var rePassword = txtRePassword.Password;

            if (isAnyFieldEmpty())
            {
                if (!areFieldsValid(email, username, password, rePassword)) return;

                User user = new User
                {
                    FirstName = firstName, LastName = lastName, Email = email, Username = username, Password = password
                };
                user = userDao.insert(user);
                if (user == null)
                {
                    Utils.ErrorBox("Failed to insert user.");
                    return;
                }

                Utils.InfoBox("Account has been successfully created.");
                action();
            }
            else
            {
                Utils.ErrorBox("Please insert all fields.");
            }
        }

        private bool areFieldsValid(string email, string username, string password, string rePassword)
        {
            if (!Utils.IsValidEmailAddress(email))
            {
                Utils.ErrorBox("Email not valid.");
                return false;
            }

            if (isUsernameFieldTaken(username))
            {
                Utils.ErrorBox("Username already in use.");
                return false;
            }

            if (isEmailFieldTaken(email))
            {
                Utils.ErrorBox("Email already in use.");
                return false;
            }

            if (password != rePassword)
            {
                Utils.ErrorBox("Passwords do not match.");
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
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Password;
            var rePassword = txtRePassword.Password;

            return firstName != "" && lastName != "" && email != "" && username != "" && password != ""
                   && rePassword != "";
        }
    }
}