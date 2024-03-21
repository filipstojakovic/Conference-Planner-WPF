using System;
using System.Collections.Generic;
using ConferenceApp.model.dto;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao
{
    public class ConferenceDao : BaseDao
    {
        public List<Conference> findAll()
        {
            var sql = @"
				SELECT *, gr.name as gr_name FROM conference c
                  JOIN gathering g ON g.id = c.gathering_id
                  LEFT JOIN user_gathering_role ugr ON g.id = ugr.gathering_id
                  LEFT JOIN user u ON ugr.user_id = u.id
                  LEFT JOIN gathering_role gr ON ugr.gathering_role_id=gr.id
				";

            List<Conference> list;
            using (var command = new MySqlCommand(sql, connection))
            {
                list = extractConferenceData(command);
            }

            return list;
        }
        
        public List<Conference> findAllWithUserId(int? userId)
        {
            var sql = @"
				SELECT *, gr.name as gr_name FROM conference c
                  JOIN gathering g ON g.id = c.gathering_id
                  LEFT JOIN user_gathering_role ugr ON g.id = ugr.gathering_id
                  LEFT JOIN user u ON ugr.user_id = u.id
                  LEFT JOIN gathering_role gr ON ugr.gathering_role_id=gr.id
				WHERE ugr.user_id=@userId
				";

            List<Conference> list;
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@userId", userId);
                list = extractConferenceData(command);
            }

            return list;
        }

        private List<Conference> extractConferenceData(MySqlCommand command)
        {
            List<Conference> conferences = new List<Conference>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var id = reader.GetInt32(reader.GetOrdinal("id"));
                    var name = reader.GetString(reader.GetOrdinal("name"));
                    var desc = reader.GetString(reader.GetOrdinal("description"));
                    var start = reader.GetDateTime(reader.GetOrdinal("start_date"));
                    var end = reader.GetDateTime(reader.GetOrdinal("end_date"));

                    Conference conference = conferences.Find(g => g.Id == id);

                    if (conference == null)
                    {
                        conference = new Conference
                        {
                            Id = id,
                            Name = name,
                            Description = desc,
                            StartDate = start,
                            EndDate = end,
                        };
                        conferences.Add(conference);
                    }

                    if (Utils.isColumnNull(reader, "user_id") == null)
                    {
                        continue;
                    }

                    UserDto user = new UserDto
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("user_id")),
                        FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                        LastName = reader.GetString(reader.GetOrdinal("last_name")),
                        Email = reader.GetString(reader.GetOrdinal("email")),
                        Username = reader.GetString(reader.GetOrdinal("username")),
                        conferenceRole = reader.GetString(reader.GetOrdinal("gr_name"))
                    };
                    conference.Users.Add(user);
                }
            }

            return conferences;
        }

        public Conference insertConference(Conference conference, MySqlTransaction transaction)
        {
            const string gatheringSql = @"INSERT INTO gathering (description) VALUES (@description)";
            const string conferenceSql =
                @"INSERT INTO conference (gathering_id, name, start_date, end_date) VALUES (@gathering_id, @name, @startDate, @endDate)";

            int gatheringId;
            using (var command = new MySqlCommand(gatheringSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@description", conference.Description);
                command.ExecuteNonQuery();
                gatheringId = (int)command.LastInsertedId;
            }

            using (var command = new MySqlCommand(conferenceSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@gathering_id", gatheringId);
                command.Parameters.AddWithValue("@name", conference.Name);
                command.Parameters.AddWithValue("@startDate", conference.StartDate);
                command.Parameters.AddWithValue("@endDate", conference.EndDate);
                command.ExecuteNonQuery();
            }

            conference.Id = gatheringId;
            return conference;
        }

        public void updateConference(Conference conference, MySqlTransaction transaction)
        {
            const string updateGatheringSql = @"
                UPDATE gathering 
                SET 
                    description = @description 
                WHERE id = @id";

            const string updateConferenceSql = @"
                UPDATE conference 
                SET
                    name = @name, 
                    start_date = @startDate,
                    end_date = @endDate
                WHERE gathering_id = @id";

            using (var command = new MySqlCommand(updateGatheringSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@id", conference.Id);
                command.Parameters.AddWithValue("@description", conference.Description);
                command.ExecuteNonQuery();
            }

            using (var command = new MySqlCommand(updateConferenceSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@id", conference.Id);
                command.Parameters.AddWithValue("@name", conference.Name);
                command.Parameters.AddWithValue("@startDate", conference.StartDate);
                command.Parameters.AddWithValue("@endDate", conference.EndDate);
                command.ExecuteNonQuery();
            }
        }

        public void deleteConference(int? gatheringId)
        {
            string deleteConferenceSql = $"DELETE FROM gathering WHERE id = {gatheringId}";

            using (var command = new MySqlCommand(deleteConferenceSql, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}