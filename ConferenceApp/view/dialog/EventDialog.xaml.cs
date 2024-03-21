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
	private readonly Session _session;

	public EventDialog(Session session, bool isReadOnly = false, Event myEvent = null)
	{
		InitializeComponent();
		_session = session;
		eventTypeDao = new EventTypeDao();
		startDatePicker.DisplayDateStart = session.StartDate;
		startDatePicker.DisplayDateEnd = session.EndDate;

		Button.Content = myEvent != null ? "Save" : "Create";
		this.Title = (myEvent != null ? "Save" : "Create") + " conference";

		EventDialogModel = createEventDialogModel(session, isReadOnly, myEvent);
		DataContext = EventDialogModel;

		var eventTypes = eventTypeDao.findAll();
		ComboBox.ItemsSource = eventTypes;
		if (myEvent != null)
		{
			var index = eventTypes.FindIndex(x => x.Id == myEvent.EventType.Id);
			ComboBox.SelectedIndex = index;
		}
	}

	private EventDialogModel createEventDialogModel(Session session, bool isReadOnly, Event myEvent)
	{
		EventDialogModel = new EventDialogModel();
		if (myEvent == null)
		{
			myEvent = new Event
			{
				StartDate = session.StartDate.AddMinutes(1),
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
		if (!(_session.StartDate <= EventDialogModel.EventDialog.StartDate
			  && _session.EndDate >= EventDialogModel.EventDialog.EndDate))
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