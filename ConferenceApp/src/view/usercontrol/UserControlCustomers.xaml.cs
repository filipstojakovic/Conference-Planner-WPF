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
            calendar.FirstDayOfWeek = DayOfWeek.Monday;
            calendar.SelectedDates.Add(new DateTime(2023,02,20));
        }
    }
}
