using ConferenceApp.model.entity;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao;

public class LocationDao : BaseDao
{
    public Location insertLocation(Location location, MySqlTransaction transaction)
    {
        LocationType buildingType = new LocationTypeDao().findByName(LocationTypeEnum.building.ToString(), transaction);

        var locationSql = @"INSERT INTO location (location_type_id, city, street) 
                            VALUES (@locationTypeId,@city,@street)";

        using (var command = new MySqlCommand(locationSql, connection, transaction))
        {
            command.Parameters.AddWithValue("@locationTypeId", buildingType.Id);
            command.Parameters.AddWithValue("@city", location.City);
            command.Parameters.AddWithValue("@street", location.Street);
            command.ExecuteNonQuery();
            location.Id = (int)command.LastInsertedId;
        }

        return location;
    }

    public Location updateLocation(Location location, MySqlTransaction transaction)
    {
        LocationType buildingType = new LocationTypeDao().findByName(LocationTypeEnum.building.ToString(), transaction);
        var sql = @"UPDATE location SET
                    location_type_id=@locationTypeId,
                    city=@city,
                    street=@street
                    WHERE id=@locationId
                    ";

        using (var command = new MySqlCommand(sql, connection, transaction))
        {
            command.Parameters.AddWithValue("@locationId", location.Id);
            command.Parameters.AddWithValue("@locationTypeId", buildingType.Id);
            command.Parameters.AddWithValue("@city", location.City);
            command.Parameters.AddWithValue("@street", location.Street);
            command.ExecuteNonQuery();
        }

        return location;
    }
}