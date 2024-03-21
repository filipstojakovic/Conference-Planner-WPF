using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ConferenceApp.model;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using ConferenceApp.view.dialog;
using Haley.Utils;

namespace ConferenceApp.view.usercontrol;

public partial class SessionControl : UserControl
{
    private BindingList<Conference> conferenceBindingList;
    private BindingList<Session> sessionBindingList;

    private Conference SelectedConference;

    private readonly ConferenceDao conferenceDao;
    private readonly SessionDao sessionDao;
    private readonly User CurrentUser;
    private readonly bool isAdmin;

    public SessionControl(User currentUser)
    {
        InitializeComponent();
        sessionDao = new SessionDao();
        conferenceDao = new ConferenceDao();
        CurrentUser = currentUser;
        isAdmin = currentUser.isAdmin();

        loadData();
    }

    private void loadData()
    {
        List<Conference> conferenceList = isAdmin ? conferenceDao.findAll() : conferenceDao.findAllWithUserId(CurrentUser.Id);
        conferenceBindingList = new BindingList<Conference>(conferenceList);
        DataContext = conferenceBindingList;
        ComboBox.DataContext = conferenceBindingList;
        ComboBox.ItemsSource = conferenceList;

        if (conferenceBindingList.Count == 0)
            Create_Button.IsEnabled = false;
    }

    private void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedConference = (Conference)ComboBox.SelectedItem;
        if (SelectedConference != null)
        {
            var sessions = sessionDao.findByConferenceId(SelectedConference.Id);
            sessionBindingList = new BindingList<Session>(sessions);
            SessionDataGrid.ItemsSource = sessionBindingList;
            SessionDataGrid.DataContext = sessionBindingList;
            CollectionViewSource.GetDefaultView(SessionDataGrid.ItemsSource).Refresh();

            var isOrganizerOrModerator = SelectedConference.isOrganizerOrModeratorUserId(CurrentUser.Id);
            Create_Button.IsEnabled = isOrganizerOrModerator;
        }
        else
        {
            SessionDataGrid.ItemsSource = new BindingList<Session>();
            SessionDataGrid.DataContext = new BindingList<Session>();
            CollectionViewSource.GetDefaultView(SessionDataGrid.ItemsSource).Refresh();
            ComboBox.SelectedIndex = 0;
        }
    }

    private void Create_Button_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedConference == null)
        {
            Utils.ErrorBox("First select conference");
            return;
        }

        var dialog = new SessionDialog(SelectedConference);
        if (dialog.ShowDialog() == true)
        {
            try
            {
                var sessionDialogData = dialog.SessionModel.getDialogResult();
                var session = sessionDao.insertSession(sessionDialogData);
                sessionBindingList.Add(session);
                CollectionViewSource.GetDefaultView(SessionDataGrid.ItemsSource).Refresh();
            }
            catch (Exception)
            {
                Utils.ErrorBox("Unable to add session");
            }
        }
    }

    private void Info_MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedSession = getSelectedConference(sender);
        var dialog = new SessionDialog(SelectedConference, selectedSession, true, true);
        dialog.ShowDialog();
    }

    private void Edit_MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedSession = getSelectedConference(sender);
        var dialog = new SessionDialog(SelectedConference, selectedSession, true);
        if (dialog.ShowDialog() == true)
        {
            try
            {
                Session session = dialog.SessionModel.getDialogResult();
                sessionDao.updateSession(session);
                selectedSession.copy(session);
                CollectionViewSource.GetDefaultView(SessionDataGrid.ItemsSource).Refresh();
            }
            catch (Exception)
            {
                Utils.ErrorBox("Unable to update session");
            }
        }
    }

    private void Delete_MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedSession = getSelectedConference(sender);
        var result = Utils.confirmAction(LangUtils.Translate("confirm_delete") + " " + selectedSession.Name);
        try
        {
            if (result)
            {
                sessionDao.deleteSession(selectedSession.Id);
                sessionBindingList.Remove(selectedSession);
                CollectionViewSource.GetDefaultView(SessionDataGrid.ItemsSource).Refresh();
            }
        }
        catch (Exception)
        {
            Utils.ErrorBox("Unable to delete session");
        }
    }

    private Session getSelectedConference(object sender)
    {
        var menuItem = (MenuItem)sender;
        var contextMenu = (ContextMenu)menuItem.Parent;
        var item = (DataGrid)contextMenu.PlacementTarget;
        return (Session)item.SelectedCells[0].Item;
    }

    private void SessionDataGrid_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
    {
        DataGridRow selectedRow = SessionDataGrid.ItemContainerGenerator
            .ContainerFromItem(SessionDataGrid.SelectedItem) as DataGridRow;
        if (selectedRow == null)
            return;

        Session rowData = selectedRow.Item as Session;
        if (rowData == null)
            return;

        var isOrganizerOrModerator = SelectedConference.isOrganizerOrModeratorUserId(CurrentUser.Id);

        if (isAdmin || isOrganizerOrModerator)
        {
            EditMenuItem.Visibility = Visibility.Visible;
            DeleteMenuItem.Visibility = Visibility.Visible;
        }
        else
        {
            EditMenuItem.Visibility = Visibility.Collapsed;
            DeleteMenuItem.Visibility = Visibility.Collapsed;
        }
    }
}