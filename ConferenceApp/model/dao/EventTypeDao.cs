using System.Collections.Generic;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao;

public class EventTypeDao : BaseDao
{
    public List<EventType> findAll()
    {
        var sql = "SELECT * FROM event_type";
        List<EventType> list;
        using (var command = new MySqlCommand(sql, connection))
        {
            list = extractEventTypeData(command);
        }

        return list;
    }

    public List<EventType> findByName(string name)
    {
        var sql = "SELECT * FROM event_type WHERE name=@name";
        List<EventType> list;
        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@name", name);
            list = extractEventTypeData(command);
        }

        return list;
    }

    private List<EventType> extractEventTypeData(MySqlCommand command)
    {
        List<EventType> list = new List<EventType>();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var id = Utils.readerGetValue<int>(reader, "id");
                var name = Utils.readerGetValue<string>(reader, "name");

                EventType eventType = new EventType()
                {
                    Id = id,
                    Name = name,
                };
                list.Add(eventType);
            }
        }

        return list;
    }
}