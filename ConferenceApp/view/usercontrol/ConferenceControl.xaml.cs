using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using ConferenceApp.model;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using ConferenceApp.view.dialog;
using Haley.Utils;
using Calendar = System.Windows.Controls.Calendar;

namespace ConferenceApp.view.usercontrol
{
	public partial class ConferenceControl : UserControl
	{
		private BindingList<Conference> conferenceBindingList;
		private readonly User currentUser;
		private readonly ConferenceDao conferenceDao;

		public ConferenceControl(User currentUser)
		{
			this.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag); //jer kalendar jede govna
			InitializeComponent();

			this.currentUser = currentUser;
			conferenceDao = new ConferenceDao();
			loadData();
		}


		private void loadData()
		{
			List<Conference> conferenceList = conferenceDao.findAll();
			conferenceBindingList = new BindingList<Conference>(conferenceList);
			conferenceBindingList.ListChanged += ConferenceBindingListOnListChanged;
			conferenceDataGrid.ItemsSource = conferenceBindingList;

			ConferenceBindingListOnListChanged(null, null);
		}

		private void ConferenceBindingListOnListChanged(object? sender, ListChangedEventArgs e)
		{
			calendar.BlackoutDates.Clear();
			foreach (var conference in conferenceBindingList)
			{
				calendar.BlackoutDates.Add(new CalendarDateRange(conference.StartDate, conference.EndDate));
			}
		}

		private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			var tbx = sender as TextBox;
			var text = tbx.Text.Trim();
			if (text != "")
			{
				var filteredList = conferenceBindingList.Where(x => x.Name.ToLower().Contains(text.ToLower()));
				conferenceDataGrid.ItemsSource = null;
				conferenceDataGrid.ItemsSource = filteredList;
			}
			else
			{
				conferenceDataGrid.ItemsSource = conferenceBindingList;
			}

			CollectionViewSource.GetDefaultView(conferenceDataGrid.Items).Refresh();
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

		private void Create_Button_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new ConferenceDialog(currentUser, null, conferenceBindingList);
			if (dialog.ShowDialog() == true)
			{
				var transaction = conferenceDao.startTransaction();
				try
				{
					Conference conference = conferenceDao.insertConference(dialog.ConferenceDialogData, transaction);
					UserGatheringRoleDao userGatheringRoleDao = new UserGatheringRoleDao();
					userGatheringRoleDao.insertUserConferenceConferenceRole(currentUser, conference,
						GatheringRoleEnum.Organizer, transaction);

					userGatheringRoleDao.insertUserConferenceConferenceRole(dialog.ConferenceModel.SelectedUser,
						conference,
						GatheringRoleEnum.Moderator, transaction);

					transaction.Commit();
					conferenceBindingList.Add(conference);
					CollectionViewSource.GetDefaultView(conferenceDataGrid.ItemsSource).Refresh();
					ConferenceBindingListOnListChanged(null, null);

				}
				catch (Exception exception)
				{
					transaction.Rollback();
					var message = LangUtils.Translate("error_insert_conference");
					Utils.ErrorBox(message);
				}

				CollectionViewSource.GetDefaultView(conferenceBindingList).Refresh();
			}
		}

		private void Edit_MenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			Conference conference = getSelectedConference(sender);
			var dialog = new ConferenceDialog(currentUser, conference, conferenceBindingList, true);
			if (dialog.ShowDialog() == true)
			{
				var transaction = conferenceDao.startTransaction();
				try
				{
					conferenceDao.updateConference(dialog.ConferenceDialogData, transaction);
					UserGatheringRoleDao userGatheringRoleDao = new UserGatheringRoleDao();
					userGatheringRoleDao.deleteConferenceModerator(conference, transaction);
					userGatheringRoleDao.insertUserConferenceConferenceRole(dialog.ConferenceModel.SelectedUser,
						conference,
						GatheringRoleEnum.Moderator, transaction);

					transaction.Commit();
					conference.copy(dialog.ConferenceDialogData);
					CollectionViewSource.GetDefaultView(conferenceDataGrid.ItemsSource).Refresh();
					ConferenceBindingListOnListChanged(null, null);
				}
				catch (Exception)
				{
					transaction.Rollback();
					var message = LangUtils.Translate("error_update_conference");
					Utils.ErrorBox(message);
				}
			}
		}

		private void Delete_MenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			Conference conference = getSelectedConference(sender);
			try
			{
				var yes = Utils.confirmAction($"Are you sure you want to delete {conference.Name}");
				if (!yes)
					return;
				conferenceDao.deleteConference(conference.Id);
				conferenceBindingList.Remove(conference);
				CollectionViewSource.GetDefaultView(conferenceDataGrid.ItemsSource).Refresh();
			}
			catch (Exception)
			{
				Utils.ErrorBox("Unable to delete " + conference.Name);
			}
		}


		private void Join_MenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			Conference conference = getSelectedConference(sender);
			if (!Utils.confirmAction("Are you sure you wanna join " + conference.Name + " conference"))
			{
				return;
			}

			UserGatheringRoleDao userGatheringRoleDao = new UserGatheringRoleDao();
			var transaction = userGatheringRoleDao.startTransaction();
			try
			{
				userGatheringRoleDao.insertUserConferenceConferenceRole(currentUser, conference,
					GatheringRoleEnum.Visitor, transaction);
				transaction.Commit();
			}
			catch (Exception)
			{
				transaction.Rollback();
				Utils.ErrorBox("Unable to join");
			}
		}

		private void Info_MenuItem_OnClick(object sender, RoutedEventArgs e)
		{
			Conference conference = getSelectedConference(sender);
			var dialog = new ConferenceUserListDialog(conference);
			dialog.ShowDialog();
		}

		private Conference getSelectedConference(object sender)
		{
			var menuItem = (MenuItem)sender;
			var contextMenu = (ContextMenu)menuItem.Parent;
			var item = (DataGrid)contextMenu.PlacementTarget;
			return (Conference)item.SelectedCells[0].Item;
		}
	}
}