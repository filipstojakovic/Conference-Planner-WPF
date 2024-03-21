using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ConferenceApp.model.entity;
using Microsoft.VisualBasic.CompilerServices;
using Utils = ConferenceApp.utils.Utils;

namespace ConferenceApp.view.dialog;

public class SessionModel : INotifyPropertyChanged
{
    private Session _session;
    private bool _isReadOnly = false;
    private DateTime _startDateSM;
    private DateTime _endDateSM;
    private DateTime _startTimeSM;
    private DateTime _endTimeSM;

    public Session Session
    {
        get { return _session; }
        set
        {
            _session = value;
            _startDateSM = _session.StartDate;
            _startTimeSM = _session.StartDate;
            _endDateSM = _session.EndDate;
            _endTimeSM = _session.EndDate;
            OnPropertyChanged();
        }
    }

    public Session getDialogResult()
    {
        _session.StartDate = getStartDateTime();
        _session.EndDate = getEndDateTime();
        return _session;
    }

    public DateTime getStartDateTime()
    {
        return Utils.combineDateAndTime(_startDateSM, _startTimeSM);
    }

    public DateTime getEndDateTime()
    {
        return Utils.combineDateAndTime(_endDateSM, _endTimeSM);
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

    public DateTime StartDateSM
    {
        get { return _startDateSM; }
        set
        {
            _startDateSM = value;
            OnPropertyChanged();
        }
    }

    public DateTime EndDateSM
    {
        get { return _endDateSM; }
        set
        {
            _endDateSM = value;
            OnPropertyChanged();
        }
    }

    public DateTime StartTimeSM
    {
        get { return _startTimeSM; }
        set
        {
            _startTimeSM = value;
            OnPropertyChanged();
        }
    }

    public DateTime EndTimeSM
    {
        get { return _endTimeSM; }
        set
        {
            _endTimeSM = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}