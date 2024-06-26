﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using ConferenceApp.model;
using ConferenceApp.model.dao;
using ConferenceApp.model.dto;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using ConferenceApp.view.dialog;
using Haley.Utils;
using Calendar = System.Windows.Controls.Calendar;

namespace ConferenceApp.view.usercontrol
{
    public partial class ConferenceControl : UserControl
    {
        private BindingList<Conference> conferenceBindingList;
        private readonly User currentUser;
        private readonly ConferenceDao conferenceDao;
        private readonly UserGatheringRoleDao userGatheringRoleDao;

        public ConferenceControl(User currentUser)
        {
            this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag); //jer kalendar jede govna
            InitializeComponent();

            this.currentUser = currentUser;
            conferenceDao = new ConferenceDao();
            userGatheringRoleDao = new UserGatheringRoleDao();
            loadData();
        }


        private void loadData()
        {
            List<Conference> conferenceList = conferenceDao.findAll();
            conferenceBindingList = new BindingList<Conference>(conferenceList);
            conferenceBindingList.ListChanged += ConferenceBindingListOnListChanged;
            conferenceDataGrid.ItemsSource = conferenceBindingList;

            ConferenceBindingListOnListChanged(null, null);
        }

        private void ConferenceBindingListOnListChanged(object? sender, ListChangedEventArgs e)
        {
            calendar.BlackoutDates.Clear();
            foreach (var conference in conferenceBindingList)
            {
                conference.checkIfUserJoined(currentUser.Id);
                calendar.BlackoutDates.Add(new CalendarDateRange(conference.StartDate, conference.EndDate));
            }
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var tbx = sender as TextBox;
            var text = tbx.Text.Trim();
            if (text != "")
            {
                var filteredList = conferenceBindingList.Where(x => x.Name.ToLower().Contains(text.ToLower()));
                conferenceDataGrid.ItemsSource = null;
                conferenceDataGrid.ItemsSource = filteredList;
            }
            else
            {
                conferenceDataGrid.ItemsSource = conferenceBindingList;
            }

            CollectionViewSource.GetDefaultView(conferenceDataGrid.Items).Refresh();
        }


        private void calendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            //DateTime date = e.AddedDate.Value;
            Calendar calObj = sender as Calendar;
            DateTime? date = calObj.DisplayDate;
            // date je prvi dan u mjesecu trenutnog prikaza
            Console.WriteLine();
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar calObj = sender as Calendar;
            SelectedDatesCollection dates = calObj.SelectedDates;
            foreach (DateTime date in dates)
            {
                Console.WriteLine();
            }

            // date is currently selected date on calendar;
            Console.WriteLine();
        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ConferenceDialog(currentUser, null, conferenceBindingList);
            if (dialog.ShowDialog() == true)
            {
                var transaction = conferenceDao.startTransaction();
                try
                {
                    Conference conference = conferenceDao.insertConference(dialog.ConferenceDialogData, transaction);
                    UserGatheringRoleDao userGatheringRoleDao = new UserGatheringRoleDao();
                    userGatheringRoleDao.insertUserConferenceConferenceRole(currentUser, conference,
                        GatheringRoleEnum.Organizer, transaction);

                    userGatheringRoleDao.insertUserConferenceConferenceRole(dialog.ConferenceModel.SelectedUser,
                        conference,
                        GatheringRoleEnum.Moderator, transaction);

                    transaction.Commit();
                    conferenceBindingList.Add(conference);
                    CollectionViewSource.GetDefaultView(conferenceDataGrid.ItemsSource).Refresh();
                    ConferenceBindingListOnListChanged(null, null);
                    loadData();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    var message = LangUtils.Translate("error_insert_conference");
                    Utils.ErrorBox(message);
                }

                CollectionViewSource.GetDefaultView(conferenceBindingList).Refresh();
            }
        }

        private void Edit_MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Conference conference = getSelectedConference(sender);
            var dialog = new ConferenceDialog(currentUser, conference, conferenceBindingList, true);
            if (dialog.ShowDialog() == true)
            {
                var transaction = conferenceDao.startTransaction();
                try
                {
                    conferenceDao.updateConference(dialog.ConferenceDialogData, transaction);
                    UserGatheringRoleDao userGatheringRoleDao = new UserGatheringRoleDao();
                    userGatheringRoleDao.deleteConferenceModerator(conference, transaction);
                    userGatheringRoleDao.insertUserConferenceConferenceRole(dialog.ConferenceModel.SelectedUser,
                        conference,
                        GatheringRoleEnum.Moderator, transaction);

                    transaction.Commit();
                    conference.copy(dialog.ConferenceDialogData);
                    CollectionViewSource.GetDefaultView(conferenceDataGrid.ItemsSource).Refresh();
                    loadData();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    var message = LangUtils.Translate("error_update_conference");
                    Utils.ErrorBox(message);
                }
            }
        }

        private void Delete_MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Conference conference = getSelectedConference(sender);
            try
            {
                var yes = Utils.confirmAction(LangUtils.Translate("conf_confirm_delete") + ": " + conference.Name + "?");
                if (!yes)
                    return;
                conferenceDao.deleteConference(conference.Id);
                conferenceBindingList.Remove(conference);
                CollectionViewSource.GetDefaultView(conferenceDataGrid.ItemsSource).Refresh();
            }
            catch (Exception)
            {
                Utils.ErrorBox("Unable to delete " + conference.Name);
            }
        }


        private void Join_MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Conference conference = getSelectedConference(sender);
            if (!Utils.confirmAction(LangUtils.Translate("conf_confirm_join") + " " + conference.Name))
            {
                return;
            }

            UserGatheringRoleDao userGatheringRoleDao = new UserGatheringRoleDao();
            var transaction = userGatheringRoleDao.startTransaction();
            try
            {
                userGatheringRoleDao.insertUserConferenceConferenceRole(currentUser, conference,
                    GatheringRoleEnum.Visitor, transaction);
                transaction.Commit();

                loadData();
            }
            catch (Exception)
            {
                transaction.Rollback();
                Utils.ErrorBox("Unable to join");
            }
        }

        private void Info_MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Conference conference = getSelectedConference(sender);
            var dialog = new ConferenceUserListDialog(conference);
            dialog.ShowDialog();
        }

        private Conference getSelectedConference(object sender)
        {
            var menuItem = (MenuItem)sender;
            var contextMenu = (ContextMenu)menuItem.Parent;
            var item = (DataGrid)contextMenu.PlacementTarget;
            return (Conference)item.SelectedCells[0].Item;
        }

        private void ConferenceDataGrid_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            DataGridRow selectedRow = conferenceDataGrid.ItemContainerGenerator
                .ContainerFromItem(conferenceDataGrid.SelectedItem) as DataGridRow;
            if (selectedRow == null)
                return;

            Conference rowData = selectedRow.Item as Conference;
            if (rowData == null)
                return;

            var isAdmin = currentUser.isAdmin();
            var isOrganizerOrModerator = rowData.isOrganizerOrModeratorUserId(currentUser.Id);
            EditMenuItem.Visibility = isAdmin || isOrganizerOrModerator ? Visibility.Visible : Visibility.Collapsed;
            DeleteMenuItem.Visibility = isAdmin || isOrganizerOrModerator ? Visibility.Visible : Visibility.Collapsed;

            if (isAdmin || isOrganizerOrModerator)
            {
                JoinMenuItem.Visibility = Visibility.Collapsed;
                LeaveMenuItem.Visibility = Visibility.Collapsed;
            }
            else
            {
                var joined = rowData.Users.Where(u => u.Id == currentUser.Id).ToArray();
                JoinMenuItem.Visibility = joined.Length == 0 ? Visibility.Visible : Visibility.Collapsed;
                LeaveMenuItem.Visibility = joined.Length != 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void LeaveMenuItem_OnClick_MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Conference conference = getSelectedConference(sender);
            var users = conference.Users.Where(u => u.Id == currentUser.Id);
            var isOrganizerOrModerator = users.Any(u => u.conferenceRole == GatheringRoleEnum.Moderator.ToString()
                                                        || u.conferenceRole == GatheringRoleEnum.Organizer.ToString());
            if (isOrganizerOrModerator)
            {
                Utils.ErrorBox(LangUtils.Translate("conf_error_admin_mod"));
                return;
            }

            userGatheringRoleDao.deleteUserGatherRole(conference.Id, currentUser.Id);
            loadData();
        }
    }
}