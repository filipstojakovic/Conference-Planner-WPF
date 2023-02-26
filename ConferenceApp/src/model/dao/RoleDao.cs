using System;
using ConferenceApp.model.entity;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao
{
	public class RoleDao : BaseDao
	{
		public Role findRoleByName(string roleName)
		{
			String sql = @"
				SELECT * FROM role
				WHERE name = @name";

			Role role = null;
			using (var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@name", roleName);
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					role = new Role
					{
						Id = reader.GetInt32(reader.GetOrdinal("id")),
						Name = reader.GetString(reader.GetOrdinal("name")),
					};
				}
			}
			return role;
		}
	}
}
