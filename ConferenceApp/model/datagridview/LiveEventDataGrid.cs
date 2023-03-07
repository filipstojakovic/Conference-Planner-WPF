using System;

namespace ConferenceApp.model.datagridview;

public class LiveEventDataGrid
{
    public int? Id { get; set; }
    public int? SessionId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public string EventTypeName { get; set; }
    public string RoomNumber { get; set; }
    public string Street { get; set; }
}