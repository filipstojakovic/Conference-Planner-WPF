using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using Microsoft.VisualBasic.CompilerServices;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao
{
    public class UserDao : BaseDao
    {
        private readonly UserRoleDao userRoleDao;
        private readonly SettingsDao settingsDao;

        public UserDao()
        {
            this.userRoleDao = new UserRoleDao();
            this.settingsDao = new SettingsDao();
        }

        public List<User> findAllUsersAndRoles()
        {
            const string sql = @"
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

        public User findById(int? id)
        {
            User user;
            const string sql = @"
				SELECT user.id as user_id, role.id as role_id, user.*, role.* FROM user
				JOIN user_has_role ON user.id = user_has_role.user_id
				JOIN role ON role.id = user_has_role.role_id
				WHERE user_id = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                List<User> users = extractUsersData(command);
                user = getFirstOrNull(users);
            }

            return user;
        }

        public User findByUsername(string username)
        {
            User user = null;
            const string sql = @"
				SELECT user.id as user_id, role.id as role_id, user.*, role.* FROM user
				JOIN user_has_role ON user.id = user_has_role.user_id
				JOIN role ON role.id = user_has_role.role_id
				WHERE username = @username";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                List<User> users = extractUsersData(command);
                user = getFirstOrNull(users);
            }

            SettingsEntity settingsEntity = settingsDao.findByUserId(user.Id);
            user.SettingsEntity = settingsEntity;

            return user;
        }

        private User getFirstOrNull(List<User> users)
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
                        Username = reader.GetString(reader.GetOrdinal("username")),
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

        public User insert(User user)
        {
            string insertSql = @"
                INSERT INTO user (username, password, first_name, last_name, email) 
                VALUES (@username, @password, @firstName, @lastName, @email)";
            var encodedPassword = Base64Util.Base64Encode(user.Password);

            var transaction = startTransaction();
            try
            {
                using (var command = new MySqlCommand(insertSql, connection, transaction))
                {
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@password", encodedPassword);
                    command.Parameters.AddWithValue("@firstName", user.FirstName);
                    command.Parameters.AddWithValue("@lastName", user.LastName);
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.ExecuteNonQuery();
                    user.Id = (int)command.LastInsertedId;
                }

                userRoleDao.insertUserRole(user, transaction);
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return null;
            }

            return findById(user.Id);
        }

        public User update(User user)
        {
            const string updateUserSql = @"
                UPDATE user 
                SET 
                    first_name = @firstName, 
                    last_name = @lastName,
                    username = @username,
                    email = @email,
                    password = @password
                WHERE id = @id";

            MySqlTransaction transaction = connection.BeginTransaction();
            try
            {
                userRoleDao.deleteUserRole(user.Id, transaction);
                userRoleDao.insertUserRole(user, transaction);
                using (var command = new MySqlCommand(updateUserSql, connection, transaction))
                {
                    command.Parameters.AddWithValue("@firstName", user.FirstName);
                    command.Parameters.AddWithValue("@lastName", user.LastName);
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@email", user.Email);
                    command.Parameters.AddWithValue("@password", user.Password);

                    command.Parameters.AddWithValue("@id", user.Id);

                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Trace.WriteLine(ex.Message);
            }

            return user;
        }

        public void delete(int? userId)
        {
            string deleteUserSql = $"DELETE FROM user WHERE id = {userId}";

            using (var command = new MySqlCommand(deleteUserSql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        public string getUserFullname_Procedure(int? id)
        {
            string fullName = "";
            using (MySqlCommand command = new MySqlCommand("get_user_fullname", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@userId", id);

                MySqlParameter fullNameParameter = new MySqlParameter("@full_name", MySqlDbType.VarChar, 255);
                fullNameParameter.Direction = ParameterDirection.Output;
                command.Parameters.Add(fullNameParameter);

                command.ExecuteNonQuery();
                fullName = (string)fullNameParameter.Value;
            }

            return fullName;
        }

        public List<User> findUserByGatherAndRole(Conference conference, GatheringRoleEnum gatheringRoleEnum)
        {
            GatheringRole gatheringRole = new GatheringRoleDao().findByName(gatheringRoleEnum.ToString());

            List<User> users = null;
            const string sql = @"
				SELECT user.id as user_id, role.id as role_id, user.*, role.* FROM user
				JOIN user_has_role ON user.id = user_has_role.user_id
				JOIN role ON role.id = user_has_role.role_id
				JOIN user_gathering_role ugr ON user.id = ugr.user_id
                WHERE ugr.gathering_id=@gatheringId AND ugr.gathering_role_id=@gatheringRoleId
				";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@gatheringId", conference.Id);
                command.Parameters.AddWithValue("@gatheringRoleId", gatheringRole.Id);

                users = extractUsersData(command);
            }

            return users;
        }
    }
}