using System.ComponentModel;

namespace ConferenceApp.model;

public class BooleanNotifier : INotifyPropertyChanged
{
    private bool _value;

    public bool Value
    {
        get { return _value; }
        set
        {
            if (_value != value)
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}