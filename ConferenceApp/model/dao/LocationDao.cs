using ConferenceApp.model.entity;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao;

public class LocationDao : BaseDao
{
    // var result = insertLocation(location,room); result.Item1 or result.Item2
    public Location insertLocation(Location location, MySqlTransaction transaction)
    {
        var locationSql = @"INSERT INTO location (location_type_id, country, city, street) 
                            VALUES (@locationTypeId,@country,@city,@street)";

        LocationType buildingType = new LocationTypeDao().findByName(LocationTypeEnum.building.ToString(), transaction);

        using (var command = new MySqlCommand(locationSql, connection, transaction))
        {
            command.Parameters.AddWithValue("@locationTypeId", buildingType.Id);
            command.Parameters.AddWithValue("@country", location.Country);
            command.Parameters.AddWithValue("@city", location.City);
            command.Parameters.AddWithValue("@street", location.Street);
            command.ExecuteNonQuery();
            location.Id = (int)command.LastInsertedId;
        }

        return location;
    }
}