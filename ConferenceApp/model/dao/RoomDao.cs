using ConferenceApp.model.entity;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao;

public class RoomDao : BaseDao
{
    private readonly LocationDao locationDao;

    public RoomDao()
    {
        locationDao = new LocationDao();
    }

    public Room insertRoom(Room room, MySqlTransaction transaction)
    {
        var sql = "INSERT INTO room (location_id,room_number,capacity) VALUES (@locationId,@roomNumber,@capacity)";

        Location location = locationDao.insertLocation(room.Location, transaction);

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

    public Room updateRoom(Room room, MySqlTransaction transaction)
    {
        Location location = locationDao.updateLocation(room.Location, transaction);

        var sql =
            @"UPDATE room SET 
                location_id=@locationId,
                room_number=@roomNumber,
                capacity=@capacity 
            WHERE id=@roomId";

        using (var command = new MySqlCommand(sql, connection, transaction))
        {
            command.Parameters.AddWithValue("@roomId", room.Id);
            command.Parameters.AddWithValue("@locationId", room.Location.Id);
            command.Parameters.AddWithValue("@roomNumber", room.RoomNumber);
            command.Parameters.AddWithValue("@capacity", room.Capacity ?? 100);
            command.ExecuteNonQuery();
            room.Id = (int)command.LastInsertedId;
        }

        return room;
    }
}