using System;
using System.Windows;
using System.Windows.Controls;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.dialog;

public partial class EventDialog : Window
{
    
    private EventType SelectedEventType;

    private EventTypeDao eventTypeDao;
    public EventDialog(Event myEvent = null)
    {
        InitializeComponent();
        eventTypeDao = new EventTypeDao();

        var eventTypes = eventTypeDao.findAll();
        ComboBox.ItemsSource = eventTypes;
    }
    
    private void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SelectedEventType = (EventType)ComboBox.SelectedItem;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Console.WriteLine();
        DialogResult = true;
        Close();
    }

    private void Cancel_Button_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}