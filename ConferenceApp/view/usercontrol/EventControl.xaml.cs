﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.view.dialog;

namespace ConferenceApp.view.usercontrol;

public partial class EventControl : UserControl
{
    private BindingList<Event> eventBindingList;
    private BindingList<Session> sessionBindingList;
    private Session SelectedSession;


    private readonly SessionDao sessionDao;
    private readonly EventDao eventDao;

    public EventControl(User currentUser)
    {
        InitializeComponent();
        sessionDao = new SessionDao();
        eventDao = new EventDao();
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
        }
    }

    private void Create_Button_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new EventDialog();
        if (dialog.ShowDialog() == true)
        {
            
        }
        
    }

    private void Edit_MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private void Delete_MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
    }
}