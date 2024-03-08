using ConferenceApp.model.entity;
using ConferenceApp.Properties;
using MySql.Data.MySqlClient;

namespace ConferenceApp.model.dao;

public class SettingsDao : BaseDao
{
    public SettingsEntity findByUserId(int? userId)
    {
        var sql = "SELECT * FROM settings WHERE user_id=@userId";
        SettingsEntity settingsEntity = null;
        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@userId", userId);
            settingsEntity = extractSettings(command);
        }

        settingsEntity.UserId = userId;
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
                UserId = reader.GetInt32(reader.GetOrdinal("id")),
                Language = reader.GetString(reader.GetOrdinal("language")),
                Theme = reader.GetString(reader.GetOrdinal("theme")),
            };
        }

        return settingsEntity;
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