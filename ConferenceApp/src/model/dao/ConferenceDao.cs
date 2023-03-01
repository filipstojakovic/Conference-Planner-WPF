using System;
using System.Collections.Generic;
using System.Diagnostics;
using ConferenceApp.model.entity;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao
{
	public class ConferenceDao : BaseDao
	{
		public List<Conference> findAll()
		{
			var sql = @"
				SELECT * FROM conference c
				JOIN geathering g ON g.id = c.geathering_id";

			List<Conference> list;
			using (var command = new MySqlCommand(sql, connection))
			{
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
					Conference conference = new Conference
					{
						Id = reader.GetInt32(reader.GetOrdinal("id")),
						Name = reader.GetString(reader.GetOrdinal("name")),
						Description = reader.GetString(reader.GetOrdinal("description")),
						StartDate = reader.GetDateTime(reader.GetOrdinal("start_date")),
						EndDate = reader.GetDateTime(reader.GetOrdinal("end_date")),
					};
					conferences.Add(conference);
				}
			}
			return conferences;
		}

		public Conference insertConference(Conference conference)
		{
			string geatheringSql = @"INSERT INTO geathering (description) VALUES (@description)";
			string conferenceSql = @"INSERT INTO conference (geathering_id, name, start_date, end_date) VALUES (@id, @name, @startDate, @endDate)";

			MySqlTransaction transaction = connection.BeginTransaction();

			try
			{
				int geatheringId;
				using (var command = new MySqlCommand(geatheringSql, connection, transaction))
				{
					command.Parameters.AddWithValue("@description", conference.Description);
					command.ExecuteNonQuery();
					geatheringId = (int)command.LastInsertedId;
				}
				using (var command = new MySqlCommand(conferenceSql, connection, transaction))
				{
					command.Parameters.AddWithValue("@id", geatheringId);
					command.Parameters.AddWithValue("@name", conference.Name);
					command.Parameters.AddWithValue("@startDate", conference.StartDate);
					command.Parameters.AddWithValue("@endDate", conference.EndDate);
					command.ExecuteNonQuery();
				}

				transaction.Commit();
				conference.Id = geatheringId;
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				Trace.WriteLine(ex.ToString());
			}
			return conference;
		}
	}
}
