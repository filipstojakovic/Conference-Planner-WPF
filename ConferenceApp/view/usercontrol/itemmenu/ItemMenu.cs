using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace ConferenceApp.view.usercontrol
{
    public class ItemMenu
    {
        public string Header { get; private set; }
        public PackIconKind Icon { get; private set; }
        public UserControl Screen { get; private set; }

        public ItemMenu(string header, PackIconKind icon, UserControl screen)
        {
            Header = header;
            Icon = icon;
            Screen = screen;
        }
    }
}