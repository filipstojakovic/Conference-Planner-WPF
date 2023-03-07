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

        EventType eventType = eventTypeDao.findByName(liveEvent.EventTypeName)[0];

        using (var command = new MySqlCommand(sql, connection, transaction))
        {
            command.Parameters.AddWithValue("@sessionId", liveEvent.SessionId);
            command.Parameters.AddWithValue("@eventTypeId", eventType.Id);
            command.Parameters.AddWithValue("@name", liveEvent.Name);
            command.Parameters.AddWithValue("@description", liveEvent.Description);
            command.Parameters.AddWithValue("@startDate", liveEvent.StartDate);
            command.Parameters.AddWithValue("@endDate", liveEvent.EndDate);
            command.ExecuteNonQuery();
            liveEvent.Id = (int)command.LastInsertedId;
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