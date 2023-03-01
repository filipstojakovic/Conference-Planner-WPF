using System;
using ConferenceApp.src.model.entity;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao
{
	public class GeatheringRoleDao : BaseDao
	{
		public GeatheringRole findByName(string geatheringRoleName)
		{
			String sql = @"
				SELECT * FROM geathering_role
				WHERE name = @name";

			GeatheringRole geatheringRole = null;
			using (var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@name", geatheringRoleName);
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					geatheringRole = new GeatheringRole
					{
						Id = reader.GetInt32(reader.GetOrdinal("id")),
						Name = reader.GetString(reader.GetOrdinal("name")),
					};
				}
			}
			return geatheringRole;
		}

		public GeatheringRole findById(int geatheringRoleId)
		{
			String sql = @"
				SELECT * FROM geathering_role
				WHERE id = @id";

			GeatheringRole geatheringRole = null;
			using (var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@id", geatheringRoleId);
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					geatheringRole = new GeatheringRole
					{
						Id = reader.GetInt32(reader.GetOrdinal("id")),
						Name = reader.GetString(reader.GetOrdinal("name")),
					};
				}
			}
			return geatheringRole;
		}
	}
}
