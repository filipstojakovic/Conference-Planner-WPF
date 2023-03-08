using ConferenceApp.model.entity;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao;

public abstract class EventDao : BaseDao
{
    protected EventTypeDao eventTypeDao;

    protected EventDao()
    {
        eventTypeDao = new EventTypeDao();
    }

    protected LiveEvent insertEvent(LiveEvent liveEvent, MySqlTransaction transaction)
    {
        var sql = @"INSERT INTO event(session_id, event_type_id, name, description, start_date, end_date)
                    VALUES (@sessionId, @eventTypeId, @name,@description,@startDate,@endDate)";

        using (var command = new MySqlCommand(sql, connection, transaction))
        {
            command.Parameters.AddWithValue("@sessionId", liveEvent.SessionId);
            command.Parameters.AddWithValue("@eventTypeId", liveEvent.EventType.Id);
            command.Parameters.AddWithValue("@name", liveEvent.Name);
            command.Parameters.AddWithValue("@description", liveEvent.Description);
            command.Parameters.AddWithValue("@startDate", liveEvent.StartDate);
            command.Parameters.AddWithValue("@endDate", liveEvent.EndDate);
            command.ExecuteNonQuery();
            liveEvent.Id = (int)command.LastInsertedId;
        }

        return liveEvent;
    }

    protected LiveEvent updateEvent(LiveEvent liveEvent, MySqlTransaction transaction)
    {
        var sql = @"UPDATE event SET 
                 session_id = @sessionId,
                event_type_id = @eventTypeId, 
                name = @name, 
                description = @description, 
                start_date = @startDate, 
                end_date = @endDate
            WHERE id = @eventId";
        
        using (var command = new MySqlCommand(sql, connection, transaction))
        {
            command.Parameters.AddWithValue("@eventId", liveEvent.Id);
            command.Parameters.AddWithValue("@sessionId", liveEvent.SessionId);
            command.Parameters.AddWithValue("@eventTypeId", liveEvent.EventType.Id);
            command.Parameters.AddWithValue("@name", liveEvent.Name);
            command.Parameters.AddWithValue("@description", liveEvent.Description);
            command.Parameters.AddWithValue("@startDate", liveEvent.StartDate);
            command.Parameters.AddWithValue("@endDate", liveEvent.EndDate);
            command.ExecuteNonQuery();
        }

        return liveEvent;
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
}