using System.Windows;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.dialog;

public partial class ConferenceDialog : Window
{
    public Conference ConferenceDialogData { get; set; }
    public Role SelectedRole { get; set; }
    public bool Edit { get; set; }

    public ConferenceDialog(Conference? conference = null, bool edit = false)
    {
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
        DialogResult = true;
        Close();
    }

    private void Cancel_Button_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}