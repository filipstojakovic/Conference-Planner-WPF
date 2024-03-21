using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using ConferenceApp.view.dialog;
using Haley.Utils;

namespace ConferenceApp.view.usercontrol;

public partial class EventControl : UserControl
{
    private BindingList<Event> eventBindingList;
    private BindingList<Session> sessionBindingList;
    private Session SelectedSession;

    private readonly ConferenceDao conferenceDao;
    private readonly SessionDao sessionDao;
    private readonly EventDao eventDao;
    private readonly LiveEventDao liveEventDao;
    private readonly OnlineEventDao onlineEventDao;

    private readonly User CurrentUser;
    private readonly bool isAdmin;


    public EventControl(User currentUser)
    {
        InitializeComponent();
        CurrentUser = currentUser;
        isAdmin = currentUser.isAdmin();
        conferenceDao = new ConferenceDao();
        sessionDao = new SessionDao();
        eventDao = new EventDao();
        liveEventDao = new LiveEventDao();
        onlineEventDao = new OnlineEventDao();
        loadData();
    }

    private void loadData()
    {
        List<Session> sessions = new();
        if (isAdmin)
        {
            sessions = sessionDao.findAll();
        }
        else
        {
            sessions = sessionDao.findAllWithUserId(CurrentUser.Id);
        }

        sessionBindingList = new BindingList<Session>(sessions);
        ComboBox.DataContext = sessionBindingList;
        ComboBox.ItemsSource = sessions;

        if (sessionBindingList.Count == 0)
            Create_Button.IsEnabled = false;
    }

    private void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedSession = (Session)ComboBox.SelectedItem;
        if (SelectedSession != null)
        {
            var events = eventDao.findBySessionId(SelectedSession.Id);
            eventBindingList = new BindingList<Event>(events);
            EventDataGrid.ItemsSource = eventBindingList;
            EventDataGrid.DataContext = eventBindingList;
            CollectionViewSource.GetDefaultView(EventDataGrid.ItemsSource).Refresh();

            Conference conference = conferenceDao.findById(SelectedSession.GatheringId);
            var isOrganizerOrMod = conference.isOrganizerOrModeratorUserId(CurrentUser.Id);
            Create_Button.IsEnabled = (isAdmin || isOrganizerOrMod);
        }
        else
        {
            EventDataGrid.ItemsSource = new BindingList<Event>();
            EventDataGrid.DataContext = new BindingList<Event>();
            CollectionViewSource.GetDefaultView(EventDataGrid.ItemsSource).Refresh();
            ComboBox.SelectedIndex = 0;
        }
    }

    private void Create_Button_Click(object sender, RoutedEventArgs e)
    {
        Session session = (Session)ComboBox.SelectedItem;
        if (session == null)
            return;
        var dialog = new EventDialog(session);
        if (dialog.ShowDialog() == true)
        {
            var transaction = eventDao.startTransaction();
            try
            {
                var result = dialog.EventDialogModel;
                Event newEvent = null;
                if (result.IsLive)
                {
                    newEvent = liveEventDao.insertLiveEvent(result.EventDialog, result.City, result.Address, transaction);
                }
                else
                {
                    newEvent = onlineEventDao.insertOnlineEvent(result.EventDialog, result.Url, transaction);
                }

                eventBindingList.Add(newEvent);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                Utils.ErrorBox("Unable to create event");
            }
        }
    }

    private void Info_MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        Session session = (Session)ComboBox.SelectedItem;
        Event selectedLiveEvent = getSelectedEvent(sender);
        //Event copy = new Event(selectedLiveEvent);
        bool isReadOnly = true;
        var dialog = new EventDialog(session, isReadOnly, selectedLiveEvent);
        if (dialog.ShowDialog() == true)
        {
        }
    }

    private void Delete_MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        Event liveEventDataGrid = getSelectedEvent(sender);
        var result = Utils.confirmAction(LangUtils.Translate("confirm_delete") + " " + liveEventDataGrid.Name);
        if (!result)
            return;
        try
        {
            eventDao.deleteEvent(liveEventDataGrid.Id);
            eventBindingList.Remove(liveEventDataGrid);
        }
        catch (Exception)
        {
            Utils.ErrorBox("Unable to delete event");
        }
    }

    private Event getSelectedEvent(object sender)
    {
        var menuItem = (MenuItem)sender;
        var contextMenu = (ContextMenu)menuItem.Parent;
        var item = (DataGrid)contextMenu.PlacementTarget;
        return (Event)item.SelectedCells[0].Item;
    }
}