using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ConferenceApp.model;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using Haley.Utils;

namespace ConferenceApp.view.dialog;

public partial class ConferenceDialog : Window
{
	public Conference ConferenceDialogData { get; set; }
	public Role SelectedRole { get; set; }
	public bool Edit { get; set; }

	private UserDao userDao;
	public ConferenceModel ConferenceModel;

	private readonly BindingList<Conference> conferenceListForChecking;

	private User currentUser;

	public ConferenceDialog(User currentUser, Conference conference = null, BindingList<Conference> conferenceListForChecking = null,
		bool edit = false)
	{
		this.currentUser = currentUser;
		InitializeComponent();
		this.conferenceListForChecking = conferenceListForChecking;
		userDao = new UserDao();

		Button.Content = edit ? LangUtils.Translate("save") : LangUtils.Translate("create");
		this.Title = (edit ? LangUtils.Translate("save") : LangUtils.Translate("create")) + " " + LangUtils.Translate("conference");

		var userList = userDao.findAllUsersAndRoles();
		ConferenceModel = new ConferenceModel(userList);
		ComboBox.DataContext = ConferenceModel;

		if (conference != null)
		{
			ConferenceDialogData = new Conference(conference);
			var previousModerator = userDao.findUserByGatherAndRole(conference, GatheringRoleEnum.Moderator).First();
			ConferenceModel.SelectedUser = previousModerator;
			var index = userList.FindIndex(x => x.Username == previousModerator.Username);
			ComboBox.SelectedIndex = index;
		}
		else
		{
			ConferenceDialogData = new Conference();
		}

		DataContext = ConferenceDialogData;
	}

	private void Button_Click(object sender, RoutedEventArgs e)
	{
		if (ConferenceDialogData.Name == "")
		{
			Utils.ErrorBox("Conference name is missing");
			return;
		}

		if (ConferenceDialogData.StartDate > ConferenceDialogData.EndDate)
		{
			Utils.ErrorBox("Start date is after end date");
			return;
		}

		if (ConferenceModel.SelectedUser == null)
		{
			Utils.ErrorBox("Please select moderator");
			return;
		}

		if (conferenceListForChecking != null)
		{
			var hasOverLap = conferenceListForChecking.Any(conference => ConferenceDialogData.Id != conference.Id
				&& Utils.DateRangesOverlap(conference.StartDate, conference.EndDate, ConferenceDialogData.StartDate,
					ConferenceDialogData.EndDate));
			if (hasOverLap)
			{
				Utils.ErrorBox("Already have conference in that time period!");
				return;
			}
		}

		DialogResult = true;
		Close();
	}

	private void Cancel_Button_Click(object sender, RoutedEventArgs e)
	{
		DialogResult = false;
		Close();
	}

}