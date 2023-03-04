using System;
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
                // var id = reader.GetInt32(reader.GetOrdinal("id"));
                // var gatheringId = reader.GetInt32(reader.GetOrdinal("gathering_id"));
                // var name = reader.GetString(reader.GetOrdinal("name"));
                // var desc = reader.GetString(reader.GetOrdinal("description"));
                // var start = reader.GetDateTime(reader.GetOrdinal("start_date"));
                // var end = reader.GetDateTime(reader.GetOrdinal("end_date"));


                Session conference = new Session()
                {
                    Id = id,
                    GatheringId = gatheringId,
                    Name = name,
                    Description = desc,
                    StartDate = start,
                    EndDate = end,
                };
                list.Add(conference);
            }
        }

        return list;
    }
}