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

		public Role insertRole(string roleName)
		{
			String sql = "INSERT INTO role(name) VALUES(@name)";

			Role role = null;
			MySqlTransaction transaction = connection.BeginTransaction();
			try
			{
				using (var command = new MySqlCommand(sql, connection, transaction))
				{
					command.Parameters.AddWithValue("@name", roleName);
					command.ExecuteNonQuery();
					int roleId = (int)command.LastInsertedId;
					role = new Role { Id = roleId, Name = roleName };
				}

				transaction.Commit();
			}
			catch (Exception ex)
			{
				transaction.Rollback();
			}

			return role;

		}
	}
}
