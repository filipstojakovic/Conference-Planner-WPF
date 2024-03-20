using System;

namespace ConferenceApp.model.entity;

public class LiveEvent : Event
{
    public string City { get; set; }
    public string Address { get; set; }

    public LiveEvent()
    {
    }

    public LiveEvent(LiveEvent liveEvent)
    {
        copy(liveEvent);
    }

    public void copy(LiveEvent _event)
    {
        this.copy(_event);
        City = _event.City;
        Address = _event.Address;
    }
}