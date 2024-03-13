namespace ConferenceApp.model.entity;

public class SettingsEntity
{
    public int? UserId { get; set; }
    public string Language { get; set; } = LanguageEnum.en.ToString();
    public string Theme { get; set; } = ThemeEnum.@default.ToString();
}