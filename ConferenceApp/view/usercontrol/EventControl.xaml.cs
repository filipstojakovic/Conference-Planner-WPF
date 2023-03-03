using System.Windows.Controls;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.usercontrol;

public partial class EventControl : UserControl
{
    public EventControl(User currentUser)
    {
        InitializeComponent();
    }
}