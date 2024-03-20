using System;
using System.Collections.Generic;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao;

public class LiveEventDao : EventDao
{

    public LiveEventDao()
    {
    }
    
    
    public Event insertLiveEvent(Event _event, string city, string address, MySqlTransaction transaction)
    {
        var newEvent = insertEvent(_event, transaction);
        
        var insertLiveEventSql = "INSERT INTO live_event (event_id, city, address) VALUES (@eventId, @city, @address)";
        using (var command = new MySqlCommand(insertLiveEventSql, connection, transaction))
        {
            command.Parameters.AddWithValue("@eventId", newEvent.Id);
            command.Parameters.AddWithValue("@city", city);
            command.Parameters.AddWithValue("@address", address);

            command.ExecuteNonQuery();
        }

        return findByEventId(newEvent.Id);
    }
    
}