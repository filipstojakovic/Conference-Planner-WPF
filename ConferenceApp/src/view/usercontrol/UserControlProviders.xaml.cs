using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;

namespace ConferenceApp.view.usercontrol
{
	/// <summary>
	/// Interaction logic for UserControlProviders.xaml
	/// </summary>
	public partial class UserControlProviders : UserControl
	{
		public UserControlProviders()
		{
			Trace.WriteLine("before init UserControlProviders");
			InitializeComponent();
			Trace.WriteLine("after init UserControlProviders");

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
	}
}