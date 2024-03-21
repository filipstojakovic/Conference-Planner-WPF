using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using Haley.Utils;

namespace ConferenceApp.view.dialog;

public partial class SessionDialog : Window
{
    public SessionModel SessionModel { get; set; }
    private List<Session> allSessions;
    private bool areAllEventsInDateRange;

    public SessionDialog(Conference conference, Session sessionDialogData = null, bool edit = false, bool isReadOnly = false)
    {
        InitializeComponent();
        
        Button.Visibility = isReadOnly ? Visibility.Collapsed : Visibility.Visible;
        var saveOrCreate = edit ? LangUtils.Translate("save") : LangUtils.Translate("create");
        Button.Content =  saveOrCreate;
        this.Title = saveOrCreate + " " + LangUtils.Translate("conference");
        
        SessionDao sessionDao = new SessionDao();
        allSessions = sessionDao.findByConferenceId(conference.Id);
        startDatePicker.DisplayDateStart = conference.StartDate;
        startDatePicker.DisplayDateEnd = conference.EndDate;

        endDatePicker.DisplayDateStart = conference.StartDate;
        endDatePicker.DisplayDateEnd = conference.EndDate;

        if (sessionDialogData == null)
        {
            sessionDialogData = new Session
            {
                GatheringId = conference.Id,
                StartDate = conference.StartDate,
                EndDate = conference.StartDate.AddMinutes(5)
            };
        }
        else
        {
            sessionDialogData = new Session(sessionDialogData);
        }

        SessionModel = new SessionModel
        {
            Session = sessionDialogData,
            IsReadOnly = isReadOnly
        };
        DataContext = SessionModel;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (SessionModel.Session.Name == "")
        {
            Utils.ErrorBox("Please give session a name");
            return;
        }

        if (SessionModel.Session.StartDate >= SessionModel.Session.EndDate)
        {
            Utils.ErrorBox("End date must be after start date!");
            return;
        }

        var hasSessionOverLap = allSessions.Any(session =>
            session.Id != SessionModel.Session.Id
            && Utils.DateRangesOverlap(session.StartDate, session.EndDate, SessionModel.Session.StartDate,
                SessionModel.Session.EndDate));
        if (hasSessionOverLap)
        {
            Utils.ErrorBox("Already have session in that time period!");
            return;
        }

        if (SessionModel.Session.Id != null)
        {
            //if editing check if there are events that are not in that time period
            var eventsFromSession = new EventDao().findBySessionId(SessionModel.Session.Id);
            areAllEventsInDateRange = eventsFromSession.All(_event => SessionModel.Session.StartDate >= _event.StartDate
                                                                      && _event.EndDate >= SessionModel.Session.EndDate);

            if (!areAllEventsInDateRange)
            {
                Utils.ErrorBox("Has events outside date/time range");
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