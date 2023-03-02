using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.usercontrol
{
    public partial class UsersControl : UserControl
    {
        private UserDao userDao;
        private BindingList<User> userBindingList;
        private Binding SelectedItem;

        public UsersControl()
        {
            InitializeComponent();

            userDao = new UserDao();

            List<User> data = userDao.findAll();
            usersList.ItemsSource = data;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(usersList.ItemsSource);
            view.Filter = UserFilter;
        }


        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;

            User user = (item as User);
            return user.FirstName.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0
                   || user.LastName.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0
                   || user.Email.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(usersList.ItemsSource).Refresh();
        }

        private void Edit_MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(e);
        }

        private void Delete_MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(e);
        }
    }
}