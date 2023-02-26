using ConferenceApp.utils;
using System.Windows;
using System.Windows.Controls;

namespace ConferenceApp.view.login;

public partial class LoginWindow : Window
{
    private AppSettings appSettings;

    public LoginWindow()
    {
        appSettings = AppSettings.getInstance();
        var lang = appSettings[AppSettings.LANG];

        InitializeComponent();


        if ("sr" == lang)
            sr.IsChecked = true;
        else
            en.IsChecked = true;

        var frame = Application.Current;
    }

    private void Login_Click(object sender, RoutedEventArgs e)
    {
        var username = this.txtUsername.Text.Trim();
        var password = this.txtPassword.Password.Trim();

        //TODO: check if normal user or admin
        if ("a" == username && "a" == password)
        {
            new MainWindow().Show(); // TODO: send logged in user
            this.Close();
        }
        else
        {
            //ERROR MESSAGE, user doesnt exist
        }
    }

    private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
    {
        var radioButton = sender as RadioButton;
        if (radioButton == null)
            return;

        appSettings.changeLang("en" == radioButton.Name ? "en" : "sr");
    }
}