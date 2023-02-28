using System;
using System.Windows.Controls;

namespace ConferenceApp.view.usercontrol
{
	/// <summary>
	/// Interaction logic for UserControlCustomers.xaml
	/// </summary>
	public partial class UserControlCustomers : UserControl
	{
		public UserControlCustomers()
		{
			InitializeComponent();

			//calendar.SelectedDates.Add(new DateTime(2023, 02, 20));

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
