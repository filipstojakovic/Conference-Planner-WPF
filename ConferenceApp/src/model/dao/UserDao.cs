using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceApp.model.entity;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao
{
	public class UserDao : BaseDao
	{
		public List<User> getAllUsers()
		{
			String sql = @"
				SELECT user.id as user_id, role.id as role_id, user.*, role.* FROM user
				JOIN user_has_role ON user.id = user_has_role.user_id
				JOIN role ON role.id = user_has_role.role_id";

			List<User> users;
			using (var command = new MySqlCommand(sql, connection))
			{
				users = extractUsersData(command);
			}

			return users;
		}

		public User getUserById(int id)
		{
			User user;
			String sql = @"
				SELECT user.id as user_id, role.id as role_id, user.*, role.* FROM user
				JOIN user_has_role ON user.id = user_has_role.user_id
				JOIN role ON role.id = user_has_role.role_id
				WHERE user_id = @id";

			using (var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@id", id);
				List<User> users = extractUsersData(command);
				user = getFirstOrThrowException(users);
			}

			return user;
		}

		public User getUserByUsername(string username)
		{
			User user = null;
			String sql = @"
				SELECT user.id as user_id, role.id as role_id, user.*, role.* FROM user
				JOIN user_has_role ON user.id = user_has_role.user_id
				JOIN role ON role.id = user_has_role.role_id
				WHERE username = @username";

			using (var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue("@username", username);
				List<User> users = extractUsersData(command);
				user = getFirstOrThrowException(users);
			}

			return user;
		}

		private User getFirstOrThrowException(List<User> users)
		{
			User user = null;
			if (users.Count == 1)
			{
				user = users[0];
			}
			return user;
		}

		private List<User> extractUsersData(MySqlCommand command)
		{
			List<User> users = new List<User>();
			using (var reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					User user = new User
					{
						Id = reader.GetInt32(reader.GetOrdinal("user_id")),
						FirstName = reader.GetString(reader.GetOrdinal("first_name")),
						LastName = reader.GetString(reader.GetOrdinal("last_name")),
						Email = reader.GetString(reader.GetOrdinal("email")),
						UserName = reader.GetString(reader.GetOrdinal("username")),
						Password = reader.GetString(reader.GetOrdinal("password"))
					};
					Role role = null;
					if (!reader.IsDBNull(reader.GetOrdinal("role_id")))
					{
						role = new Role
						{
							Id = reader.GetInt32(reader.GetOrdinal("role_id")),
							Name = reader.GetString(reader.GetOrdinal("name")),
						};
						user.Roles.Add(role);
					}

					// if the user is not yet in the list, add them
					if (!users.Any(u => u.Id == user.Id))
					{
						users.Add(user);
					}
					// otherwise, add the role to the existing user
					else if (role != null)
					{
						var existingUser = users.First(u => u.Id == user.Id);
						existingUser.Roles.Add(role);
					}
				}
			}
			return users;
		}

	}
}
