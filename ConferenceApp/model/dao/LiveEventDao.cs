using System;
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

    public LiveEvent insertLiveEvent(LiveEvent liveEvent)
    {
        var transaction = startTransaction();
        try
        {
            liveEvent.Room = roomDao.insertRoom(liveEvent.Room, transaction);
            
            
            var sql = "INSERT INTO live_event (room_id) VALUES (@roomId)";
            using (var command = new MySqlCommand(sql, connection, transaction))
            {
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