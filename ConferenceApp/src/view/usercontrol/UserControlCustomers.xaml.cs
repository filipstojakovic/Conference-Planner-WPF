using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;

namespace ConferenceApp.view.usercontrol
{
	/// <summary>
	/// Interaction logic for UserControlCustomers.xaml
	/// </summary>
	public partial class UserControlCustomers : UserControl
	{
		public UserControlCustomers()
		{
			Trace.WriteLine("before init UserControlCustomers");
			InitializeComponent();
			Trace.WriteLine("after init UserControlCustomers");

			List<User> data = new List<User>
			{
				new()
				{
					name = "Raymond Banks",
					email = "consectetuer.adipiscing.elit@hotmail.edu",
					pic = @"..\..\..\resources\images\calendar-icon.png"
				},
				new()
				{
					name = "Jonas Fletcher",
					email = "aliquet@google.net",
					pic = @"..\..\..\resources\images\calendar-icon.png"
				},
				new()
				{
					name = "Kirk Ratliff",
					email = "justo.sit.amet@icloud.edu",
					pic = @"..\..\..\resources\images\calendar-icon.png"
				},
				new()
				{
					name = "Hilary Dickerson",
					email = "cras.vulputate.velit@aol.com",
					pic = @"..\..\..\resources\images\calendar-icon.png"
				},
				new()
				{
					name = "Jackson Cain",
					email = "dictum.eu@protonmail.org",
					pic = @"..\..\..\resources\images\calendar-icon.png"
				},
				new()
				{
					name = "Raymond Banks",
					email = "consectetuer.adipiscing.elit@hotmail.edu",
					pic = @"..\..\..\resources\images\calendar-icon.png"
				},
				new()
				{
					name = "Jonas Fletcher",
					email = "aliquet@google.net",
					pic = @"..\..\..\resources\images\calendar-icon.png"
				},
				new()
				{
					name = "Kirk Ratliff",
					email = "justo.sit.amet@icloud.edu",
					pic = @"..\..\..\resources\images\calendar-icon.png"
				},
				new()
				{
					name = "Hilary Dickerson",
					email = "cras.vulputate.velit@aol.com",
					pic = @"..\..\..\resources\images\calendar-icon.png"
				},
				new()
				{
					name = "Jackson Cain",
					email = "dictum.eu@protonmail.org",
					pic = @"..\..\..\resources\images\calendar-icon.png"
				},
				new()
				{
					name = "Raymond Banks",
					email = "consectetuer.adipiscing.elit@hotmail.edu",
					pic = @"..\..\..\resources\images\calendar-icon.png"
				},
				new()
				{
					name = "Jonas Fletcher",
					email = "aliquet@google.net",
					pic = @"..\..\..\resources\images\calendar-icon.png"
				},
				new()
				{
					name = "Kirk Ratliff",
					email = "justo.sit.amet@icloud.edu",
					pic = @"..\..\..\resources\images\calendar-icon.png"
				},
				new()
				{
					name = "Hilary Dickerson",
					email = "cras.vulputate.velit@aol.com",
					pic = @"..\..\..\resources\images\calendar-icon.png"
				}
			};
			usersList.ItemsSource = data;
			CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(usersList.ItemsSource);
			view.Filter = UserFilter;
		}


		private bool UserFilter(object item)
		{
			if (String.IsNullOrEmpty(txtFilter.Text))
				return true;

			return (item as User).name.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
		}

		private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			CollectionViewSource.GetDefaultView(usersList.ItemsSource).Refresh();
		}


		public class User
		{
			public string name { get; set; }

			public string email { get; set; }

			public string pic { get; set; }
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
