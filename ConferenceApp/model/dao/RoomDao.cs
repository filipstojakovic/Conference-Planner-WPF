using ConferenceApp.model.entity;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao;

public class RoomDao : BaseDao
{
    private LocationDao locationDao;

    public RoomDao()
    {
        locationDao = new LocationDao();
    }

    public Room insertRoom(Room room, MySqlTransaction transaction)
    {
        var sql = "INSERT INTO room (location_id,room_number,capacity) VALUES (@locationId,@roomNumber,@capacity)";

        Location location = locationDao.insertLocation(room.Location,transaction);
        
        using (var command = new MySqlCommand(sql, connection, transaction))
        {
            command.Parameters.AddWithValue("@locationId", room.Location.Id);
            command.Parameters.AddWithValue("@roomNumber", room.RoomNumber);
            command.Parameters.AddWithValue("@capacity", room.Capacity ?? 100);
            command.ExecuteNonQuery();
            room.Id = (int)command.LastInsertedId;
        }

        return room;
    }
    
}