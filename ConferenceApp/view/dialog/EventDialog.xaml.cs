using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using Haley.Utils;

namespace ConferenceApp.view.dialog;

public partial class EventDialog : Window
{
    public EventDialogModel EventDialogModel { get; set; }

    private EventTypeDao eventTypeDao;
    private readonly Session SelectedSession;

    public EventDialog(Session selectedSession, bool isReadOnly = false, Event myEvent = null)
    {
        InitializeComponent();
        SelectedSession = selectedSession;
        eventTypeDao = new EventTypeDao();
        startDatePicker.DisplayDateStart = selectedSession.StartDate;
        startDatePicker.DisplayDateEnd = selectedSession.EndDate;

        var saveOrCreate = myEvent != null ? LangUtils.Translate("save") : LangUtils.Translate("create");
        Button.Content = saveOrCreate;
        this.Title = saveOrCreate + " " + LangUtils.Translate("event");
        Button.Visibility = isReadOnly ? Visibility.Collapsed : Visibility.Visible;

        EventDialogModel = createEventDialogModel(selectedSession, isReadOnly, myEvent);
        DataContext = EventDialogModel;

        var eventTypes = eventTypeDao.findAll();
        ComboBox.ItemsSource = eventTypes;
        if (myEvent != null)
        {
            var index = eventTypes.FindIndex(x => x.Id == myEvent.EventType.Id);
            ComboBox.SelectedIndex = index;
        }
        else
        {
            EventDialogModel.EventDialog.EventType = (EventType)ComboBox.SelectedItem;
        }
    }

    private EventDialogModel createEventDialogModel(Session session, bool isReadOnly, Event myEvent)
    {
        EventDialogModel = new EventDialogModel();
        if (myEvent == null)
        {
            myEvent = new Event
            {
                StartDate = session.StartDate,
                EndDate = session.StartDate.AddMinutes(5),
            };

            myEvent.SessionId = session.Id;
        }
        else if (myEvent is LiveEvent liveEvent)
        {
            EventDialogModel.City = liveEvent.City;
            EventDialogModel.Address = liveEvent.Address;
        }
        else if (myEvent is OnlineEvent onlineEvent)
        {
            EventDialogModel.Url = onlineEvent.Url;
            EventDialogModel.IsLive = false;
        }

        showGrid(EventDialogModel.IsLive);
        EventDialogModel.EventDialog = myEvent;

        EventDialogModel.IsReadOnly = isReadOnly;

        return EventDialogModel;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (EventDialogModel.EventDialog.StartDate > EventDialogModel.EventDialog.EndDate)
        {
            Utils.ErrorBox("Event start time is after end time");
            return;
        }

        if (!(SelectedSession.StartDate <= EventDialogModel.EventDialog.StartDate
              && SelectedSession.EndDate >= EventDialogModel.EventDialog.EndDate))
        {
            Utils.ErrorBox("Event is outside session start/end range");
            return;
        }

        if (string.IsNullOrEmpty(EventDialogModel.EventDialog.Name))
        {
            Utils.ErrorBox("Event name is missing");
            return;
        }

        if (EventDialogModel.EventDialog.EventType.Id == null)
        {
            Utils.ErrorBox("Event type is missing");
            return;
        }
        
        var eventsFromSession = new EventDao().findBySessionId(SelectedSession.Id);
        var hasEventOverLap = eventsFromSession.Any(_event =>
            _event.Id != EventDialogModel.EventDialog.Id
            && Utils.DateRangesOverlap(_event.StartDate, _event.EndDate, EventDialogModel.EventDialog.StartDate,
                EventDialogModel.EventDialog.EndDate));

        if (hasEventOverLap)
        {
            Utils.ErrorBox("Event is overlapping with another event");
            return;
        }

        bool isLive = isLiveToggleButton.IsChecked == false;
        EventDialogModel.IsLive = isLive;
        DialogResult = true;
        Close();
    }

    private void Cancel_Button_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void isLiveToggleButtonClicked(object sender, RoutedEventArgs e)
    {
        bool isLive = isLiveToggleButton.IsChecked == false;
        showGrid(isLive);
    }

    private void showGrid(bool isLive)
    {
        LiveGrid.Visibility = isLive ? Visibility.Visible : Visibility.Collapsed;
        OnlineGrid.Visibility = !isLive ? Visibility.Visible : Visibility.Collapsed;
    }
}