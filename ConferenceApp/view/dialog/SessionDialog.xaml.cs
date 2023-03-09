using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using ConferenceApp.utils;

namespace ConferenceApp.view.dialog;

public partial class SessionDialog : Window
{
    public Session SessionDialogData { get; set; }
    private List<Session> allSessions;
    private bool areAllEventsInDateRange;

    public SessionDialog(Conference conference, Session sessionDialogData = null, bool edit = false)
    {
        InitializeComponent();
        Button.Content = edit ? "Save" : "Create";
        this.Title = (edit ? "Save" : "Create") + " conference";
        SessionDao sessionDao = new SessionDao();
        allSessions = sessionDao.findByConferenceId(conference.Id);

        if (sessionDialogData == null)
        {
            sessionDialogData = new Session
            {
                GatheringId = conference.Id,
                StartDate = conference.StartDate,
                EndDate = conference.StartDate
            };
        }
        else
        {
            sessionDialogData = new Session(sessionDialogData);
        }

        this.SessionDialogData = sessionDialogData;
        DataContext = sessionDialogData;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (SessionDialogData.Name == "")
        {
            Utils.ErrorBox("Please give session a name");
            return;
        }

        if (SessionDialogData.StartDate >= SessionDialogData.EndDate)
        {
            Utils.ErrorBox("End date must be after start date!");
            return;
        }

        var hasSessionOverLap = allSessions.Any(session =>
            session.Id != SessionDialogData.Id
            && Utils.DateRangesOverlap(session.StartDate, session.EndDate, SessionDialogData.StartDate,
                SessionDialogData.EndDate));
        if (hasSessionOverLap)
        {
            Utils.ErrorBox("Already have session in that time period!");
            return;
        }

        if (SessionDialogData.Id != null)
        {
            var sessionEvents = new LiveEventDao().findBySessionId(SessionDialogData.Id);
            areAllEventsInDateRange = sessionEvents
                .All(liveEvent => SessionDialogData.StartDate >= liveEvent.StartDate 
                                  && liveEvent.EndDate >= SessionDialogData.EndDate);

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