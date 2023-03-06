using System.Collections.Generic;
using System.Windows;
using ConferenceApp.model;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.dialog;

public partial class ConferenceUserListDialog : Window
{
    public ConferenceUserListDialog(Conference conference)
    {
        InitializeComponent();

        UserDao userDao = new UserDao();
        var Moderator = userDao.findUserByGatherAndRole(conference, GatheringRoleEnum.Moderator)[0];
        ModeratorName.Text = Moderator.FullName;
        var Organiser = userDao.findUserByGatherAndRole(conference, GatheringRoleEnum.Organizer)[0];
        OrganizerName.Text = Organiser.FullName;
        conferenceDataGrid.ItemsSource = userDao.findUserByGatherAndRole(conference, GatheringRoleEnum.Visitor);
    }
}