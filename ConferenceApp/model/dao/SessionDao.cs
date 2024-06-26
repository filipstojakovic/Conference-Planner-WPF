﻿using System;
using System.Collections.Generic;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao;

public class SessionDao : BaseDao
{
    public List<Session> findAll()
    {
        var sql = "SELECT * FROM session";
        List<Session> list;
        using (var command = new MySqlCommand(sql, connection))
        {
            list = extractSessionData(command);
        }

        return list;
    }

    public List<Session> findAllWithUserId(int? userId)
    {
        var sql = "SELECT * FROM session s " +
                  "JOIN user_gathering_role ugr ON ugr.gathering_id=s.gathering_id " +
                  "WHERE ugr.user_id=@userId";
        List<Session> list;
        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@userId", userId);
            list = extractSessionData(command);
        }

        return list;
    }

    public List<Session> findByConferenceId(int? gatheringId)
    {
        var sql = "SELECT * FROM session WHERE gathering_id = @gatheringId";
        List<Session> list;
        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@gatheringId", gatheringId);
            list = extractSessionData(command);
        }

        return list;
    }

    public Session insertSession(Session session)
    {
        string insertSql = @"
                INSERT INTO session (gathering_id, name, description, start_date, end_date) 
                VALUES (@gatheringId, @name, @description, @startDate, @endDate)";

        using (var command = new MySqlCommand(insertSql, connection))
        {
            command.Parameters.AddWithValue("@gatheringId", session.GatheringId);
            command.Parameters.AddWithValue("@name", session.Name);
            command.Parameters.AddWithValue("@description", session.Description);
            command.Parameters.AddWithValue("@startDate", session.StartDate);
            command.Parameters.AddWithValue("@endDate", session.EndDate);
            command.ExecuteNonQuery();
            session.Id = (int)command.LastInsertedId;
        }

        return session;
    }

    public void updateSession(Session session)
    {
        var sql = @"UPDATE session 
                    SET
                       gathering_id=@gatheringId,
                       name=@name,
                       description=@description,
                       start_date=@startDate,
                       end_date=@endDate
                   WHERE id=@sessionId
                    ";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@sessionId", session.Id);
            command.Parameters.AddWithValue("@gatheringId", session.GatheringId);
            command.Parameters.AddWithValue("@name", session.Name);
            command.Parameters.AddWithValue("@description", session.Description);
            command.Parameters.AddWithValue("@startDate", session.StartDate);
            command.Parameters.AddWithValue("@endDate", session.EndDate);
            command.ExecuteNonQuery();
        }
    }

    public void deleteSession(int? sessionId)
    {
        var sql = $"DELETE FROM session WHERE id={sessionId}";
        using (var command = new MySqlCommand(sql, connection))
        {
            command.ExecuteNonQuery();
        }
    }

    private List<Session> extractSessionData(MySqlCommand command)
    {
        List<Session> list = new List<Session>();
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var id = Utils.readerGetValue<int>(reader, "id");
                var gatheringId = Utils.readerGetValue<int>(reader, "gathering_id");
                var name = Utils.readerGetValue<string>(reader, "name");
                var desc = Utils.readerGetValue<string?>(reader, "description");
                var start = Utils.readerGetValue<DateTime>(reader, "start_date");
                var end = Utils.readerGetValue<DateTime>(reader, "end_date");

                Session session = new Session()
                {
                    Id = id,
                    GatheringId = gatheringId,
                    Name = name,
                    Description = desc,
                    StartDate = start,
                    EndDate = end,
                };
                list.Add(session);
            }
        }

        return list;
    }
}