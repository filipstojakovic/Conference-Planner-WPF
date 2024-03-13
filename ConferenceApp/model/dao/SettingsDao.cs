using System;
using System.Transactions;
using ConferenceApp.model.entity;
using ConferenceApp.utils;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao;

public class SettingsDao : BaseDao
{
    public SettingsEntity findByUserId(int? userId)
    {
        SettingsEntity settingsEntity = null;
        if (settingsExists(userId))
        {
            var sql = "SELECT * FROM settings WHERE user_id=@userId";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@userId", userId);
                settingsEntity = extractSettings(command);
            }

            settingsEntity.UserId = userId;
            return settingsEntity;
        }

        var transaction = startTransaction();
        settingsEntity = new SettingsEntity();
        settingsEntity.UserId = userId;
        try
        {
            insertSettings(settingsEntity, transaction);
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
        }

        return settingsEntity;
    }

    private SettingsEntity extractSettings(MySqlCommand command)
    {
        SettingsEntity settingsEntity = new SettingsEntity();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            settingsEntity = new SettingsEntity
            {
                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                Language = reader.GetString(reader.GetOrdinal("language")),
                Theme = reader.GetString(reader.GetOrdinal("theme")),
            };
        }

        return settingsEntity;
    }

    public bool settingsExists(int? userId)
    {
        try
        {
            string query = "SELECT COUNT(*) FROM settings WHERE user_id = @userId";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@userId", userId);
            int count = Convert.ToInt32(command.ExecuteScalar());

            return count > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        return false;
    }

    public void createSettings(User user, MySqlTransaction transaction)
    {
        SettingsEntity settingsEntity = new SettingsEntity();
        settingsEntity.UserId = user.Id;
        insertSettings(settingsEntity, transaction);
    }

    public void insertSettings(SettingsEntity settingsEntity, MySqlTransaction transaction)
    {
        string insertSql = @"
                INSERT INTO settings (user_id, language, theme) 
                VALUES (@userId, @language, @theme)";
        using (var command = new MySqlCommand(insertSql, connection, transaction))
        {
            command.Parameters.AddWithValue("@userId", settingsEntity.UserId);
            command.Parameters.AddWithValue("@language", settingsEntity.Language);
            command.Parameters.AddWithValue("@theme", settingsEntity.Theme);
            command.ExecuteNonQuery();
        }
    }

    public void updateSettings(SettingsEntity settings)
    {
        var sql = @"UPDATE settings 
                    SET
                       language=@language,
                       theme=@theme
                   WHERE user_id=@userId
                    ";

        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@language", settings.Language);
            command.Parameters.AddWithValue("@theme", settings.Theme);
            command.Parameters.AddWithValue("@userId", settings.UserId);
            command.ExecuteNonQuery();
        }
    }
}