using System;
using System.Collections.Generic;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using MySql.Data.MySqlClient;


namespace ConferenceApp.model.dao;

public class EventDao : BaseDao
{
    protected EventTypeDao eventTypeDao = new();

    public List<Event> findBySessionId(int? sessionId)
    {
        // var sql =  $"SELECT * FROM view_live_event WHERE session_id={sessionId}";
        const string sql = $@"
                    SELECT e.*
                         , et.name    as event_type_name
                         , le.city as city
                         , le.address as address
                         , oe.url     as url
                    FROM event e
                             JOIN event_type et on e.event_type_id = et.id
                             LEFT JOIN live_event le on e.id = le.event_id
                             Left JOIN online_event oe on e.id = oe.event_id
                WHERE session_id=@sessionId";
        List<Event> events;
        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@sessionId", sessionId);
            events = extractEventData(command);
        }

        return events;
    }

    public Event findByEventId(int? eventId)
    {
        Event result = null;
        const string sql = $@"
                    SELECT e.*
                         , et.name    as event_type_name
                         , le.city as city
                         , le.address as address
                         , oe.url     as url
                    FROM event e
                             JOIN event_type et on e.event_type_id = et.id
                             LEFT JOIN live_event le on e.id = le.event_id
                             Left JOIN online_event oe on e.id = oe.event_id
                WHERE e.id=@eventId";
        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@eventId", eventId);
            result = extractEventData(command)[0];
        }

        return result;
    }


    protected Event insertEvent(Event _event, MySqlTransaction transaction)
    {
        var sql = @"INSERT INTO event(session_id, event_type_id, name, description, start_date, end_date)
                    VALUES (@sessionId, @eventTypeId, @name,@description,@startDate,@endDate)";

        using (var command = new MySqlCommand(sql, connection, transaction))
        {
            addEventParameters(_event, command);
            command.ExecuteNonQuery();
            _event.Id = (int)command.LastInsertedId;
        }

        return _event;
    }

    private void addEventParameters(Event _event, MySqlCommand command)
    {
        command.Parameters.AddWithValue("@sessionId", _event.SessionId);
        command.Parameters.AddWithValue("@eventTypeId", _event.EventType.Id);
        command.Parameters.AddWithValue("@name", _event.Name);
        command.Parameters.AddWithValue("@description", _event.Description);
        command.Parameters.AddWithValue("@startDate", _event.StartDate);
        command.Parameters.AddWithValue("@endDate", _event.EndDate);
    }

    public void deleteEvent(int? eventId)
    {
        var sql = "DELETE FROM event WHERE id=@eventId";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@eventId", eventId);
            command.ExecuteNonQuery();
        }
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

                var eventTypeId = Utils.readerGetValue<int>(reader, "event_type_id");
                var eventTypeName = Utils.readerGetValue<string>(reader, "event_type_name");

                if (!reader.IsDBNull(reader.GetOrdinal("url")))
                {
                    string url = Utils.readerGetValue<string>(reader, "url");
                    OnlineEvent onlineEvent = new OnlineEvent()
                    {
                        Id = id,
                        SessionId = sessionId,
                        Name = name,
                        Description = desc,
                        StartDate = start,
                        EndDate = end,
                        EventType = new EventType
                        {
                            Id = eventTypeId,
                            Name = eventTypeName
                        },
                        Url = url
                    };
                    list.Add(onlineEvent);
                }
                else
                {
                    var city = Utils.readerGetValue<string>(reader, "city");
                    var address = Utils.readerGetValue<string>(reader, "address");
                    LiveEvent liveEvent = new LiveEvent()
                    {
                        Id = id,
                        SessionId = sessionId,
                        Name = name,
                        Description = desc,
                        StartDate = start,
                        EndDate = end,
                        EventType = new EventType
                        {
                            Id = eventTypeId,
                            Name = eventTypeName
                        },
                        City = city,
                        Address = address
                        
                    };
                    list.Add(liveEvent);
                }
            }
        }

        return list;
    }
}