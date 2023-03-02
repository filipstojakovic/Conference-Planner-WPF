using System.Collections.Generic;
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
					var id = reader.GetInt32(reader.GetOrdinal("id"));
					var name = reader.GetString(reader.GetOrdinal("name"));
					var desc = reader.GetString(reader.GetOrdinal("description"));
					var start = reader.GetDateTime(reader.GetOrdinal("start_date"));
					var end = reader.GetDateTime(reader.GetOrdinal("end_date"));


					Conference conference = new Conference
					{
						Id = id,
						Name = name,
						Description = desc,
						StartDate = start,
						EndDate = end,
					};
					conferences.Add(conference);
				}
			}
			return conferences;
		}

		public Conference insertConference(Conference conference, MySqlTransaction transaction)
		{
			string geatheringSql = @"INSERT INTO geathering (description) VALUES (@description)";
			string conferenceSql = @"INSERT INTO conference (geathering_id, name, start_date, end_date) VALUES (@id, @name, @startDate, @endDate)";

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

			conference.Id = geatheringId;
			return conference;
		}
	}
}
