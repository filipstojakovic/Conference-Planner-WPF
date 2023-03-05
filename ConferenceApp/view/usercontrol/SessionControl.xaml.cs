using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
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
    }

    private void Delete_MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private void Edit_MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private void Create_Button_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SessionDialog();
        if (dialog.ShowDialog() == true)
        {
            
        }
    }
}