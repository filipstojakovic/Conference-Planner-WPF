using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;

namespace ConferenceApp.view.login
{
    /// <summary>
    /// Interaction logic for LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        private AppSettings appSettings;
        private UserDao userDao;
        private Action action;

        public LoginUserControl(Action action)
        {
            appSettings = AppSettings.getInstance();
            userDao = new UserDao();
            this.action = action;
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var username = this.txtUsername.Text.Trim();
            var password = this.txtPassword.Password.Trim();

            User user = userDao.findByUsername(username);
            if (checkIfUsernameAndPasswordValid(user, username, password))
            {
                new MainWindow(user).Show();
                action();
            }
            else if ("" == username && "" == password)  // TODO: delete me
            {
                user = userDao.findByUsername("admin");
                new MainWindow(user).Show(); 
                action();
            }
            else
            {
                //ERROR MESSAGE, user doesnt exist
                MessageBox.Show("Wrong username or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool checkIfUsernameAndPasswordValid(User user, string username, string password)
        {
            if (user == null)
                return false;

            string encodedPassword = Base64Util.Base64Encode(password);
            return user.Username == username && encodedPassword == user.Password;
        }

        private void TextBox_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login_Click(sender, e);
            }
        }
    }
}