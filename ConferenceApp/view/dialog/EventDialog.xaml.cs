using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;

namespace ConferenceApp.view.dialog;

public partial class EventDialog : Window
{
    public EventDialogModel EventDialogModel { get; set; }

    private EventTypeDao eventTypeDao;
    private Session _session;

    public EventDialog(Session session, LiveEvent myLiveEvent = null)
    {
        InitializeComponent();
        _session = session;
        eventTypeDao = new EventTypeDao();

        Button.Content = myLiveEvent != null ? "Save" : "Create";
        this.Title = (myLiveEvent != null ? "Save" : "Create") + " conference";

        if (myLiveEvent == null)
        {
            myLiveEvent = new LiveEvent();
            myLiveEvent.SessionId = session.Id;
        }

        EventDialogModel = new EventDialogModel();
        EventDialogModel.LiveEventDialog = myLiveEvent;
        DataContext = EventDialogModel;

        var eventTypes = eventTypeDao.findAll();
        ComboBox.ItemsSource = eventTypes;
        if (myLiveEvent != null)
        {
            var index = eventTypes.FindIndex(x => x.Id == myLiveEvent.EventType.Id);
            ComboBox.SelectedIndex = index;
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (!(_session.StartDate <= EventDialogModel.LiveEventDialog.StartDate
              && _session.EndDate >= EventDialogModel.LiveEventDialog.EndDate))
        {
            Utils.ErrorBox("Event is outside session start/end range");
            return;
        }

        DialogResult = true;
        Close();
    }

    private void Cancel_Button_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}