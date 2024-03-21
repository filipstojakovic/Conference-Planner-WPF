using System.ComponentModel;
using System.Runtime.CompilerServices;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.dialog;

public class SessionModel : INotifyPropertyChanged
{
    private Session _session;
    private bool _isReadOnly = false;

    public Session Session
    {
        get { return _session; }
        set
        {
            _session = value;
            OnPropertyChanged();
        }
    }
    
    public bool IsReadOnly
    {
        get { return _isReadOnly; }
        set
        {
            _isReadOnly = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}