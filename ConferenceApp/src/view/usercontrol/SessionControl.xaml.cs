using System.Windows.Controls;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.usercontrol;

public partial class SessionControl : UserControl
{
    public SessionControl(User currentUser)
    {
        InitializeComponent();
    }
}