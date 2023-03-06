using System;
using System.Collections.Generic;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao;

public class EventDao : BaseDao
{

    public List<Event> findAll()
    {
        //using select_events_with_event_type VIEW
        var sql = "SELECT * FROM select_events_with_event_type";
        List<Event> list;
        using (var command = new MySqlCommand(sql, connection))
        {
            list = extractEventData(command);
        }

        return list;
    }
    
    public List<Event> findBySessionId(int? sessionId)
    {
        var sql = $"SELECT * FROM select_events_with_event_type WHERE session_id={sessionId}";
        List<Event> list;
        using (var command = new MySqlCommand(sql, connection))
        {
            list = extractEventData(command);
        }

        return list;
    }
    
    private List<Event> extractEventData(MySqlCommand command)
    {
        List<Event> list = new List<Event>();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var id = Utils.readerGetValue<int>(reader, "id");
                var sessionId = Utils.readerGetValue<int>(reader, "session_id");
                var name = Utils.readerGetValue<string>(reader, "name");
                var desc = Utils.readerGetValue<string?>(reader, "description");
                var start = Utils.readerGetValue<DateTime>(reader, "start_date");
                var end = Utils.readerGetValue<DateTime>(reader, "end_date");
                
                var eventTypeName = Utils.readerGetValue<string>(reader, "event_type_name");

                Event conference = new Event()
                {
                    Id = id,
                    SessionId = sessionId,
                    Name = name,
                    Description = desc,
                    StartDate = start,
                    EndDate = end,
                    EventType = eventTypeName
                };
                list.Add(conference);
            }
        }

        return list;
    }

}