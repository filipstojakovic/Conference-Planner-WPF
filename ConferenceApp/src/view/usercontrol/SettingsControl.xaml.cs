using ConferenceApp.utils;
using System.Windows.Controls;

namespace ConferenceApp.view.usercontrol
{
	/// <summary>
	/// Interaction logic for SettingsControl.xaml
	/// </summary>
	public partial class SettingsControl : UserControl
    {
        private readonly AppSettings appSettings;
        public SettingsControl()
        {
            appSettings = AppSettings.getInstance();
            InitializeComponent();

            langComboBox.SelectedItem = langComboBox.FindName(appSettings[AppSettings.LANG]);
            themeComboBox.SelectedItem = themeComboBox.FindName(appSettings[AppSettings.THEME]);
        }

        private void LangComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lang = selectedItemName(sender);
            appSettings.changeLang(lang);
        }


        private void ThemeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var theme = selectedItemName(sender);
            appSettings.changeTheme(theme);

        }

        private string selectedItemName(object sender)
        {
            ComboBox comboBox = sender as ComboBox;
            var selectedItem = comboBox.SelectedItem as ComboBoxItem;
            return selectedItem.Name;
        }
    }
}
