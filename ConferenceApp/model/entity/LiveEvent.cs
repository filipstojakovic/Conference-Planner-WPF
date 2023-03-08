using System;

namespace ConferenceApp.model.entity;

public class LiveEvent : Event
{
    public Room Room { get; set; } = new Room();

    public LiveEvent()
    {
    }

    public LiveEvent(LiveEvent liveEvent)
    {
        copy(liveEvent);
    }

    public void copy(LiveEvent _event)
    {
        Id = _event.Id;
        SessionId = _event.SessionId;
        Name = _event.Name;
        Description = _event.Description;
        EventType = new EventType { Id = _event.EventType.Id, Name = _event.EventType.Name };
        StartDate = _event.StartDate;
        EndDate = _event.EndDate;
        Room = new Room
        {
            Id = _event.Room.Id,
            Capacity = _event.Room.Capacity,
            RoomNumber = _event.Room.RoomNumber,
            Location = new Location
            {
                Id = _event.Room.Location.Id,
                City = _event.Room.Location.City,
                Street = _event.Room.Location.Street
            }
        };
    }
}