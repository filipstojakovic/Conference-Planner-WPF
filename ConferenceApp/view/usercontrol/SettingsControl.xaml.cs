﻿using System.Diagnostics;
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

        private readonly ListView drawerUpperMenuList;
        private readonly ListView drawerBottomMenuList;

        public SettingsControl(ListView upperMenuListView, ListView bottomMenuListView)
        {
            drawerUpperMenuList = upperMenuListView;
            drawerBottomMenuList = bottomMenuListView;
            Trace.WriteLine("before init AppSettings");
            appSettings = AppSettings.getInstance();
            InitializeComponent();
            Trace.WriteLine("after init AppSettings");

            langComboBox.SelectedItem = langComboBox.FindName(appSettings[AppSettings.LANG]);
            themeComboBox.SelectedItem = themeComboBox.FindName(appSettings[AppSettings.THEME]);
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
}