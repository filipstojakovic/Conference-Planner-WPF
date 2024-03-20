using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ConferenceApp.model.dto;

namespace ConferenceApp.model.entity
{
    public class Conference : Gathering, INotifyPropertyChanged
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public BindingList<UserDto> Users { get; set; }

        public Conference()
        {
            Name = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(1);
            Users = new BindingList<UserDto>();
        }

        public Conference(Conference conference)
        {
            copy(conference);
        }

        public void copy(Conference conference)
        {
            Id = conference.Id;
            Name = conference.Name;
            Description = conference.Description;
            StartDate = conference.StartDate;
            EndDate = conference.EndDate;
        }

        public void checkIfUserJoined(int? userId)
        {
            _isJoined = Users.Any(user => user.Id == userId);
        }

        private bool _isJoined;

        public bool IsJoined
        {
            get { return _isJoined; }
            set
            {
                if (_isJoined != value)
                {
                    _isJoined = value;
                    OnPropertyChanged(nameof(IsJoined));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}