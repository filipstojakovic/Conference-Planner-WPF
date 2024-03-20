﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using ConferenceApp.view.dialog;

namespace ConferenceApp.view.usercontrol;

public partial class EventControl : UserControl
{
	private BindingList<Event> eventBindingList;
	private BindingList<Session> sessionBindingList;
	private Session SelectedSession;

	private readonly SessionDao sessionDao;
	private readonly EventDao eventDao;
	private readonly LiveEventDao liveEventDao;
	private readonly OnlineEventDao onlineEventDao;


	public EventControl(User currentUser)
	{
		InitializeComponent();
		sessionDao = new SessionDao();
		eventDao = new EventDao();
		liveEventDao = new LiveEventDao();
		onlineEventDao = new OnlineEventDao();
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
			var events = eventDao.findBySessionId(SelectedSession.Id);
			eventBindingList = new BindingList<Event>(events);
			EventDataGrid.ItemsSource = eventBindingList;
			EventDataGrid.DataContext = eventBindingList;
			CollectionViewSource.GetDefaultView(EventDataGrid.ItemsSource).Refresh();
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
			Console.WriteLine();

		}
	}

	// private void Edit_MenuItem_OnClick(object sender, RoutedEventArgs e)
	// {
	// Session session = (Session)ComboBox.SelectedItem;
	// LiveEvent selectedLiveEvent = getSelectedEvent(sender);
	// LiveEvent copy = new LiveEvent(selectedLiveEvent);
	// var dialog = new EventDialog(session, copy);
	// if (dialog.ShowDialog() == true)
	// {
	//     var transaction = liveEventDao.startTransaction();
	//     try
	//     {
	//         liveEventDao.updateLiveEvent(copy, transaction);
	//         selectedLiveEvent.copy(copy);
	//         transaction.Commit();
	//         CollectionViewSource.GetDefaultView(EventDataGrid.ItemsSource).Refresh();
	//     }
	//     catch (Exception)
	//     {
	//         transaction.Rollback();
	//         Utils.ErrorBox("Unable to update event");
	//     }
	// }
	// }

	private void Delete_MenuItem_OnClick(object sender, RoutedEventArgs e)
	{
		Event liveEventDataGrid = getSelectedEvent(sender);
		var result = Utils.confirmAction("Are you sure you want to delete " + liveEventDataGrid.Name);
		if (!result)
			return;
		try
		{
			eventDao.deleteEvent(liveEventDataGrid.Id);
			eventBindingList.Remove(liveEventDataGrid);
		}
		catch (Exception)
		{
			Utils.ErrorBox("Uaable to delete event");
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