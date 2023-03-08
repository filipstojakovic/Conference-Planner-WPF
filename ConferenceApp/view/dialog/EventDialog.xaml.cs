using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.dialog;

public partial class EventDialog : Window
{
    private EventDialogModel _eventDialogModel;

    private EventTypeDao eventTypeDao;
    public EventDialog(Session session,LiveEvent myLiveEvent = null)
    {
        InitializeComponent();
        eventTypeDao = new EventTypeDao();

        if (myLiveEvent == null)
        {
            myLiveEvent = new LiveEvent();
            myLiveEvent.SessionId = session.Id;
        }

        _eventDialogModel = new EventDialogModel();
        _eventDialogModel.LiveEventDialog = myLiveEvent;
        DataContext = _eventDialogModel;
        
        var eventTypes = eventTypeDao.findAll();
        var eventTypeNames = eventTypes.Select(x => x.Name);
        ComboBox.ItemsSource = eventTypeNames;
        ComboBox.SelectedIndex = 0;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void Cancel_Button_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}