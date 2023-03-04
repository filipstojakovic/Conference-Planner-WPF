using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using ConferenceApp.view.dialog;

namespace ConferenceApp.view.usercontrol
{
    public partial class UsersControl : UserControl
    {
        private User loggedInUser;
        private UserDao userDao;
        private BindingList<User> userBindingList;

        public UsersControl(User loggedInUser = null)
        {
            this.loggedInUser = loggedInUser;
            InitializeComponent();
            userDao = new UserDao();
            loadData();
        }

        private void loadData()
        {
            List<User> users = userDao.findAll();
            userBindingList = new BindingList<User>(users);
            usersList.ItemsSource = userBindingList;
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
            string filter = txtFilter.Text.ToLower();

            // Filter the BindingList based on the text in the textbox
            var filteredPeople = userBindingList
                .Where(user => user.FirstName.ToLower().Contains(filter) ||
                               user.LastName.ToLower().Contains(filter) ||
                               user.Email.ToString().Contains(filter))
                .ToList();

            // Set the filtered BindingList as the DataSource for the DataGridView
            usersList.ItemsSource = new BindingList<User>(filteredPeople);
            CollectionViewSource.GetDefaultView(usersList.ItemsSource).Refresh();
        }

        private void Edit_MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            User user = getSelectedUser(sender);
            var dialog = new UserDialog(user);
            if (dialog.ShowDialog() == true)
            {
                user.copy(dialog.UserDialogData);
                userDao.update(user);
                CollectionViewSource.GetDefaultView(usersList.ItemsSource).Refresh();
            }
        }

        private void Delete_MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            User user = getSelectedUser(sender);
            if (user.Id == loggedInUser.Id)
            {
                Utils.ErrorBox("You cannot delete yourself!");
                return;
            }

            var result = Utils.confirmAction($"Are you sure you want to delete {user.FirstName} {user.LastName}?");
            if (result)
            {
                userBindingList.Remove(user);
                userDao.delete(user.Id);
                loadData();
                txtFilter_TextChanged(null, null);
            }
        }


        private User getSelectedUser(object sender)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;

            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;

            //Get the underlying item, that you cast to your object that is bound
            //to the DataGrid (and has subject and state as property)
            return (User)item.SelectedCells[0].Item;
        }
    }
}