using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.dialog;

public class EventDialogData : INotifyPropertyChanged
{
    private Event _event;

    public Event EventDialog
    {
        get { return _event; }
        set
        {
            _event = value;
            OnPropertyChanged();
        }
    }
    
    

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}