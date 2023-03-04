using System.Globalization;
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

    public ConferenceDialog(Conference conference = null, bool edit = false)
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

        DialogResult = true;
        Close();
    }

    private void Cancel_Button_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}