using System;

namespace ConferenceApp.model.entity;

public class Event
{
    public int? Id { get; set; }
    public int? SessionId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public EventType EventType { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);
}