using ConferenceApp.model.entity;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao;

public class OnlineEventDao : EventDao
{
    public OnlineEventDao()
    {
    }
    
    public Event insertOnlineEvent(Event _event, string url, MySqlTransaction transaction)
    {
        var newEvent = insertEvent(_event, transaction);
        
        var insertLiveEventSql = "INSERT INTO online_event (event_id, url) VALUES (@eventId, @url)";
        using (var command = new MySqlCommand(insertLiveEventSql, connection, transaction))
        {
            command.Parameters.AddWithValue("@eventId", newEvent.Id);
            command.Parameters.AddWithValue("@url", url);

            command.ExecuteNonQuery();
        }

        return findByEventId(newEvent.Id);
    }
}