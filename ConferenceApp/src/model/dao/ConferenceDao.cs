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
				JOIN gathering g ON g.id = c.gathering_id";

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
			const string gatheringSql = @"INSERT INTO gathering (description) VALUES (@description)";
			const string conferenceSql = @"INSERT INTO conference (gathering_id, name, start_date, end_date) VALUES (@gathering_id, @name, @startDate, @endDate)";

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
	}
}
