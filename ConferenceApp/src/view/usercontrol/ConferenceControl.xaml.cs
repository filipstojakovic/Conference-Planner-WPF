using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.usercontrol
{
    public partial class ConferenceControl : UserControl
    {
        private ConferenceDao conferenceDao;
        private ObservableCollection<Conference> conferenceBindingList;

        public ConferenceControl()
        {
            Trace.WriteLine("before init UserControlCustomers");
            InitializeComponent();
            Trace.WriteLine("after init UserControlCustomers");

            conferenceDao = new ConferenceDao();
            var conferences = conferenceDao.findAll();
            conferenceBindingList = new ObservableCollection<Conference>(conferences);
            conferenceBindingList.CollectionChanged+=ConferenceBindingListOnCollectionChanged;
            conferenceList.ItemsSource = conferenceBindingList;

            conferences.ForEach(conference =>
            {
                calendar.BlackoutDates.Add(new CalendarDateRange(conference.StartDate, conference.EndDate));
            });
        }

        private void ConferenceBindingListOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine();
        }

        private void ConferenceBindingListOnListChanged(object? sender, ListChangedEventArgs e)
        {
            if (sender == null)
                return;
            Console.WriteLine(sender);
        }


        private bool ConferenceFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;

            return (item as Conference).Name.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var tbx = sender as TextBox;
            var text = tbx.Text.Trim();
            if (text != "")
            {
                var filteredList = conferenceBindingList.Where(x => x.Name.ToLower().Contains(text.ToLower()));
                conferenceList.ItemsSource = null;
                conferenceList.ItemsSource = filteredList;
            }
            else
            {
                conferenceList.ItemsSource = conferenceBindingList;
            }

            //CollectionViewSource.GetDefaultView(conferenceList.ItemsSource).Refresh();
        }


        private void calendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            //DateTime date = e.AddedDate.Value;
            Calendar calObj = sender as Calendar;
            DateTime? date = calObj.DisplayDate;
            // date je prvi dan u mjesecu trenutnog prikaza
            Console.WriteLine();
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar calObj = sender as Calendar;
            SelectedDatesCollection dates = calObj.SelectedDates;
            foreach (DateTime date in dates)
            {
                Console.WriteLine();
            }

            // date je trenutno slektovan dan na kalendaru
            Console.WriteLine();
        }

        private void Edit_MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Conference conference = getSelectedConference(sender);
            
        }

        private void Delete_MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            Conference conference = getSelectedConference(sender);
            //Remove the toDeleteFromBindedList object from your ObservableCollection
            conferenceBindingList.Remove(conference);
        }


        private Conference getSelectedConference(object sender)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;

            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;

            //Get the underlying item, that you cast to your object that is bound
            //to the DataGrid (and has subject and state as property)
            return (Conference)item.SelectedCells[0].Item;
        }
    }
}