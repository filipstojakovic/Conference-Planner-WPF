using System.Collections.Generic;
using ConferenceApp.model.dao;
using ConferenceApp.model.entity;
using MySql.Data.MySqlClient;

public class LocationTypeDao : BaseDao
{
    public List<LocationType> findAll()
    {
        const string locationTypeSql = @"SELECT * FROM location_type";

        List<LocationType> locationTypes = null;
        using (var command = new MySqlCommand(locationTypeSql, connection))
        {
            locationTypes = extractLocationTypeData(command);
        }

        return locationTypes;
    }

    public LocationType findByName(string name, MySqlTransaction transaction)
    {
        LocationType locationType;
        const string locationTypeSql = @"SELECT * FROM location_type WHERE name=@name";
        using (var command = new MySqlCommand(locationTypeSql, connection, transaction))
        {
            command.Parameters.AddWithValue("@name", name);
            var locationTypes = extractLocationTypeData(command);
            locationType = locationTypes[0];
        }

        return locationType;
    }

    private List<LocationType> extractLocationTypeData(MySqlCommand command)
    {
        List<LocationType> locationTypes = new List<LocationType>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var locationType = new LocationType
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
            };
            locationTypes.Add(locationType);
        }

        return locationTypes;
    }
}