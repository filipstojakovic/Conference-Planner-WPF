using System;
using ConferenceApp.model.entity;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao
{
	public class GatheringRoleDao : BaseDao
	{
		public GatheringRole findByName(string gatheringRoleName)
		{
			const string sql = @"
				SELECT * FROM gathering_role
				WHERE name = @name";

			GatheringRole gatheringRole = null;
			using (var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@name", gatheringRoleName);
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					gatheringRole = new GatheringRole
					{
						Id = reader.GetInt32(reader.GetOrdinal("id")),
						Name = reader.GetString(reader.GetOrdinal("name")),
					};
				}
			}
			return gatheringRole;
		}

		public GatheringRole findById(int gatheringRoleId)
		{
			const string sql = @"
				SELECT * FROM gathering_role
				WHERE id = @id";

			GatheringRole gatheringRole = null;
			using (var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@id", gatheringRoleId);
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					gatheringRole = new GatheringRole
					{
						Id = reader.GetInt32(reader.GetOrdinal("id")),
						Name = reader.GetString(reader.GetOrdinal("name")),
					};
				}
			}
			return gatheringRole;
		}
	}
}
