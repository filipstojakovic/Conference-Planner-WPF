using System;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace ConferenceApp.view.usercontrol
{
    public class ItemMenu
    {
        public string Header { get; private set; }
        public PackIconKind Icon { get; private set; }

        public readonly CreateUserControlAction CreateUserControl; 
        public delegate UserControl CreateUserControlAction();

        public ItemMenu(string header, PackIconKind icon, CreateUserControlAction createUserControl)
        {
            Header = header;
            Icon = icon;
            this.CreateUserControl = createUserControl;
        }
    }
}