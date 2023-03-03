using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ConferenceApp.model;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using ConferenceApp.view.dialog;

namespace ConferenceApp.view.usercontrol
{
    public partial class ConferenceControl : UserControl
    {
        private ConferenceDao conferenceDao;
        private List<Conference> conferenceList;
        private BindingList<Conference> conferenceBindingList;
        private User currentUser;

        public ConferenceControl(User currentUser)
        {
            Trace.WriteLine("before init UserControlCustomers");
            InitializeComponent();
            Trace.WriteLine("after init UserControlCustomers");

            this.currentUser = currentUser;
            conferenceDao = new ConferenceDao();
            loadData();
        }


        private void loadData()
        {
            conferenceList = conferenceDao.findAll();
            conferenceBindingList = new BindingList<Conference>(conferenceList);
            conferenceListDataGrid.ItemsSource = conferenceBindingList;

            conferenceList.ForEach(conference =>
            {
                calendar.BlackoutDates.Add(new CalendarDateRange(conference.StartDate, conference.EndDate));
            });
        }

        private bool ConferenceFilter(object item)
        {
            if (string.IsNullOrEmpty(txtFilter.Text))
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
                conferenceListDataGrid.ItemsSource = null;
                conferenceListDataGrid.ItemsSource = filteredList;
            }
            else
            {
                conferenceListDataGrid.ItemsSource = conferenceBindingList;
            }

            CollectionViewSource.GetDefaultView(conferenceListDataGrid).Refresh();
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

            // date is currently selected date on calendar;
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

        private void NewConference_Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ConferenceDialog();
            if (dialog.ShowDialog() == true)
            {
                var transaction = conferenceDao.startTransaction();
                try
                {
                    Conference conference = conferenceDao.insertConference(dialog.ConferenceDialogData, transaction);
                    UserGatheringRoleDao userGatheringRoleDao = new UserGatheringRoleDao();
                    userGatheringRoleDao.insertUserConferenceConferenceRole(currentUser, conference,
                        GatheringRoleEnum.Organizer, transaction);

                    transaction.Commit();
                    conferenceBindingList.Add(conference);
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    Utils.ErrorBox("Unable to insert conference!");
                }


                CollectionViewSource.GetDefaultView(conferenceListDataGrid.ItemsSource).Refresh();
            }
            else
            {
                var test = dialog.ConferenceDialogData;
            }
        }
    }
}