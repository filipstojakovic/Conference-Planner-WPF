namespace ConferenceApp.model.entity;

public class Room
{
    public int? Id { get; set; }
    public Location Location { get; set; }
    public string RoomNumber { get; set; }
    public int? Capacity { get; set; } = 100;
}