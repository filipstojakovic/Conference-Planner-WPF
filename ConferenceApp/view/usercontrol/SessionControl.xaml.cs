using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using ConferenceApp.view.dialog;

namespace ConferenceApp.view.usercontrol;

public partial class SessionControl : UserControl
{
    private BindingList<Conference> conferenceBindingList;
    private BindingList<Session> sessionBindingList;

    private Conference SelectedConference;

    private readonly ConferenceDao conferenceDao;
    private readonly SessionDao sessionDao;

    public SessionControl(User currentUser)
    {
        InitializeComponent();
        sessionDao = new SessionDao();
        conferenceDao = new ConferenceDao();

        loadData();
    }

    private void loadData()
    {
        var conferenceList = conferenceDao.findAll();
        conferenceBindingList = new BindingList<Conference>(conferenceList);
        DataContext = conferenceBindingList;
        ComboBox.DataContext = conferenceBindingList;
        ComboBox.ItemsSource = conferenceList;
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

        //TODO: check if inside conference time period
        //TODO: check events in time period
        try
        {
            var dialog = new SessionDialog(SelectedConference.Id);
            if (dialog.ShowDialog() == true)
            {
                var sessionDialogData = dialog.SessionDialogData;
                var session = sessionDao.insertSession(sessionDialogData);
                sessionBindingList.Add(session);
                CollectionViewSource.GetDefaultView(SessionDataGrid.ItemsSource).Refresh();
            }
        }
        catch (Exception)
        {
            Utils.ErrorBox("Unable to add session");
        }
    }

    private void Edit_MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedSession = getSelectedConference(sender);
        var dialog = new SessionDialog(SelectedConference.Id, selectedSession, true);
        try
        {
            if (dialog.ShowDialog() == true)
            {
                //TODO: check if inside conference time period
                //TODO: check events in time period
                Session session = dialog.SessionDialogData;
                sessionDao.updateSession(session);
                selectedSession.copy(session);
                CollectionViewSource.GetDefaultView(SessionDataGrid.ItemsSource).Refresh();
            }
        }
        catch (Exception)
        {
            Utils.ErrorBox("Unable to update session");
        }
    }

    private void Delete_MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        var selectedSession = getSelectedConference(sender);
        var result = Utils.confirmAction("Are you sure you want to delete session " + selectedSession.Name);
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
}