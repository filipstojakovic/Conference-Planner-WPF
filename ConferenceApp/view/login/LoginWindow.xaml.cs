using System.Windows;
using System.Windows.Controls;
using ConferenceApp.model.dao;
using ConferenceApp.utils;

namespace ConferenceApp.view.login;

public partial class LoginWindow : Window
{
    private AppSettings appSettings;
    private UserDao userDao;

    public LoginWindow()
    {
        appSettings = AppSettings.getInstance();
        InitializeComponent();

        DockPanelMain.Children.Clear();
        DockPanelMain.Children.Add(new LoginUserControl(closeWindow));

        var frame = Application.Current;
    }

    private void closeWindow()
    {
        this.Close();
    }

    private void goToLoginScene()
    {
        DockPanelMain.Children.Clear();
        DockPanelMain.Children.Add(new LoginUserControl(closeWindow));
        login.IsChecked = true;
    }

    private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
    {
        var radioButton = sender as RadioButton;
        if (radioButton == null || DockPanelMain == null)
            return;

        DockPanelMain.Children.Clear();
        var radioButtonName = radioButton.Name;
        if ("login" == radioButtonName)
        {
            DockPanelMain.Children.Add(new LoginUserControl(closeWindow));
        }
        else
        {
            DockPanelMain.Children.Add(new RegistrationUserControl(goToLoginScene));
        }
    }
}