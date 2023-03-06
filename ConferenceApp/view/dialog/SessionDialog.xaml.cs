using System.Windows;
using ConferenceApp.model.entity;
using ConferenceApp.utils;

namespace ConferenceApp.view.dialog;

public partial class SessionDialog : Window
{
    public Session SessionDialogData { get; set; }

    public SessionDialog(int? gatheringId, Session sessionDialogData = null, bool edit = false)
    {
        InitializeComponent();
        Button.Content = edit ? "Save" : "Create";
        this.Title = (edit ? "Save" : "Create") + " conference";

        if (sessionDialogData == null)
        {
            sessionDialogData = new Session();
            sessionDialogData.GatheringId = gatheringId;
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
        DialogResult = true;
        Close();
    }
    
    private void Cancel_Button_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}