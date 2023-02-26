using ConferenceApp.database;
using ConferenceApp.model.entity;
using MySql.Data.MySqlClient;
using System;

namespace ConferenceApp.model.dao
{
	public class UserDao
	{
		private MySqlConnection connection;

		public UserDao()
		{
			connection = MySqlSingleton.getInstance().connection;
		}

		public User getUserById(int id)
		{
			User user = null;
			String sql = @"
				SELECT user.id as user_id, role.id as role_id, user.*, role.* FROM user
				JOIN user_has_role ON user.id = user_has_role.user_id
				JOIN role ON role.id = user_has_role.role_id
				WHERE user_id = @id";

			using (var command = new MySqlCommand(sql, connection))
			{

				command.Parameters.AddWithValue("@id", id);

				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						if (user == null)
						{
							user = new User
							{
								Id = reader.GetInt32(reader.GetOrdinal("id")),
								FirstName = reader.GetString(reader.GetOrdinal("first_name")),
								LastName = reader.GetString(reader.GetOrdinal("last_name")),
								Email = reader.GetString(reader.GetOrdinal("email")),
								UserName = reader.GetString(reader.GetOrdinal("username")),
								Password = reader.GetString(reader.GetOrdinal("password"))
							};
						}
						if (!reader.IsDBNull(reader.GetOrdinal("role_id")))
						{
							Role role = new Role
							{
								Id = reader.GetInt32(reader.GetOrdinal("id")),
								Name = reader.GetString(reader.GetOrdinal("name")),
							};
							user.Roles.Add(role);
						}
					}
				}
			}

			return user;
		}
	}
}
