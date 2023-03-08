using System;
using System.Collections.Generic;
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

    public List<LiveEvent> findBySessionId(int? sessionId)
    {
        var sql = $"SELECT * FROM view_live_event WHERE session_id={sessionId}";
        List<LiveEvent> list;
        using (var command = new MySqlCommand(sql, connection))
        {
            list = extractEventData(command);
        }

        return list;
    }

    public LiveEvent insertLiveEvent(LiveEvent liveEvent, MySqlTransaction transaction)
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

        return liveEvent;
    }

    public LiveEvent updateLiveEvent(LiveEvent liveEvent, MySqlTransaction transaction)
    {
        liveEvent.Room = roomDao.updateRoom(liveEvent.Room, transaction);

        liveEvent = updateEvent(liveEvent, transaction);

        //TODO: vjerovatno je visak
        // var insertLiveEventSql = "INSERT INTO live_event (event_id, room_id) VALUES (@eventId,@roomId)";
        // using (var command = new MySqlCommand(insertLiveEventSql, connection, transaction))
        // {
        //     command.Parameters.AddWithValue("@eventId", liveEvent.Id);
        //     command.Parameters.AddWithValue("@roomId", liveEvent.Room.Id);
        //     command.ExecuteNonQuery();
        //     liveEvent.Id = (int)command.LastInsertedId;
        // }

        return liveEvent;
    }

    private List<LiveEvent> extractEventData(MySqlCommand command)
    {
        List<LiveEvent> list = new List<LiveEvent>();
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

                var roomId = Utils.readerGetValue<int>(reader, "room_id");
                var roomNumber = Utils.readerGetValue<string>(reader, "room_number");

                var locationId = Utils.readerGetValue<int>(reader, "location_id");
                var city = Utils.readerGetValue<string>(reader, "city");
                var street = Utils.readerGetValue<string>(reader, "street");

                LiveEvent conference = new LiveEvent()
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
                    Room = new Room
                    {
                        Id = roomId,
                        RoomNumber = roomNumber,
                        Location = new Location
                        {
                            Id = locationId,
                            City = city,
                            Street = street
                        }
                    }
                };
                list.Add(conference);
            }
        }

        return list;
    }
}