using System;

namespace ConferenceApp.model.entity;

public class Event
{
	public int? Id { get; set; }
	public int? SessionId { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public EventType EventType { get; set; } = new EventType();
	public DateTime StartDate { get; set; } = DateTime.Now;
	public DateTime EndDate { get; set; } = DateTime.Now;


	public Event()
	{
	}
	public Event(Event newEvent)
	{
		copy(newEvent);
	}

	public void copy(Event _event)
	{
		Id = _event.Id;
		SessionId = _event.SessionId;
		Name = _event.Name;
		Description = _event.Description;
		EventType = new EventType { Id = _event.EventType.Id, Name = _event.EventType.Name };
		StartDate = _event.StartDate;
		EndDate = _event.EndDate;
	}
}