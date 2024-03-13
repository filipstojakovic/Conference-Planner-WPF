using System.Windows.Controls;
using ConferenceApp.utils;

namespace ConferenceApp.view.usercontrol;

public partial class SettingsControl : UserControl
{
	private readonly AppSettings appSettings;

	private readonly ListView drawerUpperMenuList;
	private readonly ListView drawerBottomMenuList;

	public SettingsControl(ListView upperMenuListView, ListView bottomMenuListView)
	{
		drawerUpperMenuList = upperMenuListView;
		drawerBottomMenuList = bottomMenuListView;
		appSettings = AppSettings.getInstance();
		InitializeComponent();

		langComboBox.SelectedItem = langComboBox.FindName(appSettings.SettingsEntity.Language);
		themeComboBox.SelectedItem = themeComboBox.FindName(appSettings.SettingsEntity.Theme);
	}

	private void LangComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		var lang = selectedItemName(sender);
		appSettings.changeLang(lang);
		drawerBottomMenuList.Items.Refresh();
		drawerUpperMenuList.Items.Refresh();
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