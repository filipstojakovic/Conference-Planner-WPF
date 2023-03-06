using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ConferenceApp.model.entity;

namespace ConferenceApp.view.dialog;

public class ConferenceModel : INotifyPropertyChanged
{
    public ObservableCollection<User> UsersModel { get; set; }

    private User _selectedUser;

    public User SelectedUser
    {
        get { return _selectedUser; }
        set
        {
            _selectedUser = value;
            OnPropertyChanged();
        }
    }

    public ConferenceModel()
    {
    }

    public ConferenceModel(List<User> usersList)
    {
        UsersModel = new ObservableCollection<User>(usersList);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}