using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ConferenceApp.model.dao;
using ConferenceApp.model.datagridview;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using ConferenceApp.view.dialog;

namespace ConferenceApp.view.usercontrol;

public partial class EventControl : UserControl
{
    private BindingList<LiveEventDataGrid> eventBindingList;
    private BindingList<Session> sessionBindingList;
    private Session SelectedSession;

    private readonly SessionDao sessionDao;
    private readonly LiveEventDao liveEventDao;

    public EventControl(User currentUser)
    {
        InitializeComponent();
        sessionDao = new SessionDao();
        liveEventDao = new LiveEventDao();
        loadData();
        // if (sessionBindingList.Count > 0)
        // {
        //     ComboBox.SelectedIndex = 0;
        // }
    }

    private void loadData()
    {
        var sessions = sessionDao.findAll();
        sessionBindingList = new BindingList<Session>(sessions);
        ComboBox.DataContext = sessionBindingList;
        ComboBox.ItemsSource = sessions;
    }

    private void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedSession = (Session)ComboBox.SelectedItem;
        if (SelectedSession != null)
        {
            var events = liveEventDao.findBySessionId(SelectedSession.Id);
            eventBindingList = new BindingList<LiveEventDataGrid>(events);
            EventDataGrid.ItemsSource = eventBindingList;
            EventDataGrid.DataContext = eventBindingList;
            CollectionViewSource.GetDefaultView(EventDataGrid.ItemsSource).Refresh();
        }
        else
        {
            EventDataGrid.ItemsSource = new BindingList<LiveEvent>();
            EventDataGrid.DataContext = new BindingList<LiveEvent>();
            CollectionViewSource.GetDefaultView(EventDataGrid.ItemsSource).Refresh();
            ComboBox.SelectedIndex = 0;
        }
    }

    private void Create_Button_Click(object sender, RoutedEventArgs e)
    {
        Session session = (Session)ComboBox.SelectedItem;
        var dialog = new EventDialog(session);
        if (dialog.ShowDialog() == true)
        {
            //TODO: insert event and update list
            //TODO: check session date/time
            //1: inside session start-end period
            //2: no overlap with other events
            Console.WriteLine();
        }
    }

    private void Edit_MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private void Delete_MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        LiveEventDataGrid liveEventDataGrid = getSelectedEvent(sender);
        var result = Utils.confirmAction("Are you sure you want to delete " + liveEventDataGrid.Name);
        if (!result)
            return;
        try
        {
            liveEventDao.deleteEvent(liveEventDataGrid.Id);
            eventBindingList.Remove(liveEventDataGrid);
        }
        catch (Exception)
        {
            Utils.ErrorBox("Was not able to remove event");
        }
    }

    private LiveEventDataGrid getSelectedEvent(object sender)
    {
        var menuItem = (MenuItem)sender;
        var contextMenu = (ContextMenu)menuItem.Parent;
        var item = (DataGrid)contextMenu.PlacementTarget;
        return (LiveEventDataGrid)item.SelectedCells[0].Item;
    }

}