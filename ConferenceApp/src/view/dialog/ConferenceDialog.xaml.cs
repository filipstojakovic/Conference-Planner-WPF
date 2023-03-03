using System.Windows;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.dialog;

public partial class ConferenceDialog : Window
{
    public Conference ConferenceDialogData { get; set; }
    public Role SelectedRole { get; set; }
    public bool Edit { get; set; }
    
    public ConferenceDialog()
    {
        InitializeComponent();
    }

    private void Cancel_Button_Click(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}