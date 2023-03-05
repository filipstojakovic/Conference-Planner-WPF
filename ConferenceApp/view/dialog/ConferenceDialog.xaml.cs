using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using Haley.Utils;

namespace ConferenceApp.view.dialog;

public partial class ConferenceDialog : Window
{
    public Conference ConferenceDialogData { get; set; }
    public Role SelectedRole { get; set; }
    public bool Edit { get; set; }

    private readonly BindingList<Conference> conferenceListForChecking;

    public ConferenceDialog(Conference conference = null, BindingList<Conference> conferenceListForChecking = null,
        bool edit = false)
    {
        CultureInfo culture = new CultureInfo(LangUtils.CurrentCulture.Name);
        Thread.CurrentThread.CurrentCulture = culture;
        InitializeComponent();
        Button.Content = edit ? "Save" : "Create";
        this.Title = (edit ? "Save" : "Create") + " conference";

        if (conference != null)
            ConferenceDialogData = new Conference(conference);
        else
        {
            ConferenceDialogData = new Conference();
        }

        this.conferenceListForChecking = conferenceListForChecking;
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

        if (conferenceListForChecking != null)
        {
            var hasOverLap = conferenceListForChecking.Any(conference => Utils.DateRangesOverlap(conference.StartDate,
                conference.EndDate, ConferenceDialogData.StartDate, ConferenceDialogData.EndDate));
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