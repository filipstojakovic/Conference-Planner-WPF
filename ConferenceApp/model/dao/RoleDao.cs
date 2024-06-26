﻿using System;
using System.Collections.Generic;
using ConferenceApp.model.entity;
using MySql.Data.MySqlClient;


namespace ConferenceApp.model.dao
{
    public class RoleDao : BaseDao
    {
        public Role findByName(string roleName)
        {
            const string sql = @"
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

        public List<Role> findAll()
        {
            const string sql = "SELECT * FROM role";

            List<Role> roles = new List<Role>();
            using (var command = new MySqlCommand(sql, connection))
            {
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Role role = new Role
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                    };
                    roles.Add(role);
                }
            }

            return roles;
        }

        public Role insert(string roleName)
        {
            const string sql = "INSERT INTO role(name) VALUES(@name)";

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
            catch (Exception)
            {
                transaction.Rollback();
            }

            return role;
        }
    }
}