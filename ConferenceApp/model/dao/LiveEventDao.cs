using System;
using System.Collections.Generic;
using ConferenceApp.model.datagridview;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao;

public class LiveEventDao : EventDao
{
    private RoomDao roomDao;

    public LiveEventDao()
    {
        roomDao = new RoomDao();
    }

    public List<LiveEventDataGrid> findBySessionId(int? sessionId)
    {
        var sql = $"SELECT * FROM view_live_event WHERE session_id={sessionId}";
        List<LiveEventDataGrid> list;
        using (var command = new MySqlCommand(sql, connection))
        {
            list = extractEventData(command);
        }

        return list;
    }

    private List<LiveEventDataGrid> extractEventData(MySqlCommand command)
    {
        List<LiveEventDataGrid> list = new List<LiveEventDataGrid>();
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
                var roomNumber = Utils.readerGetValue<string>(reader, "room_number");
                var street = Utils.readerGetValue<string>(reader, "street");

                LiveEventDataGrid conference = new LiveEventDataGrid()
                {
                    Id = id,
                    SessionId = sessionId,
                    Name = name,
                    Description = desc,
                    StartDate = start,
                    EndDate = end,
                    EventTypeName = eventTypeName,
                    RoomNumber = roomNumber,
                    Street = street
                };
                list.Add(conference);
            }
        }

        return list;
    }


    public LiveEvent insertLiveEvent(LiveEvent liveEvent)
    {
        var transaction = startTransaction();
        try
        {
            liveEvent.Room = roomDao.insertRoom(liveEvent.Room, transaction);

            liveEvent = insertEvent(liveEvent, transaction);
            
            var insertLiveEventSql = "INSERT INTO live_event (event_id, room_id) VALUES (@eventId,@roomId)";
            using (var command = new MySqlCommand(insertLiveEventSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@eventId", liveEvent.Id);
                command.Parameters.AddWithValue("@roomId", liveEvent.Room.Id);
                command.ExecuteNonQuery();
                liveEvent.Id = (int)command.LastInsertedId;
            }

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Utils.ErrorBox("Unable to insert live event");
        }

        return liveEvent;
    }
}