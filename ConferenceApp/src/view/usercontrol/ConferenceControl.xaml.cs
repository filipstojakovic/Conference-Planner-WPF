using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.usercontrol
{
	public partial class ConferenceControl : UserControl
	{
		private ConferenceDao conferenceDao;
		private BindingList<Conference> conferenceBindingList;

		public ConferenceControl()
		{
			Trace.WriteLine("before init UserControlCustomers");
			InitializeComponent();
			Trace.WriteLine("after init UserControlCustomers");

			conferenceDao = new ConferenceDao();
			var conferences = conferenceDao.findAll();
			conferenceBindingList = new BindingList<Conference>(conferences);
			conferenceList.ItemsSource = conferenceBindingList;


			conferences.ForEach(conference =>
			{
				calendar.BlackoutDates.Add(new CalendarDateRange(conference.StartDate, conference.EndDate));
			});

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
	}
}
