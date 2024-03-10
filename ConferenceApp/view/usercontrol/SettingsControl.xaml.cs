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

		//TODO: check this out
		//if ("invert".Equals(theme.ToLower()))
		//{
		//    var currentFontFamily = TextBlock.FontFamilyProperty.GetMetadata(typeof(TextBlock)).DefaultValue.ToString();
		//    if (!"Comic Sans MS".Equals(currentFontFamily))
		//        TextBlock.FontFamilyProperty.OverrideMetadata(
		//            typeof(TextBlock),
		//            new FrameworkPropertyMetadata(
		//                new FontFamily("Comic Sans MS")));
		//}
	}

	private string selectedItemName(object sender)
	{
		ComboBox comboBox = sender as ComboBox;
		var selectedItem = comboBox.SelectedItem as ComboBoxItem;
		return selectedItem.Name;
	}
}