using System.Windows;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.dialog;

public partial class SessionDialog : Window
{
    public Session sessionDialogData;


    public SessionDialog(Session sessionDialogData = null, bool edit = false)
    {
        InitializeComponent();
        Button.Content = edit ? "Save" : "Create";
        this.Title = (edit ? "Save" : "Create") + " conference";

        if (sessionDialogData == null)
            sessionDialogData = new Session();
        this.sessionDialogData = sessionDialogData;
        DataContext = sessionDialogData;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        //TODO: check fields
        DialogResult = true;
        Close();
    }
    
    private void Cancel_Button_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}