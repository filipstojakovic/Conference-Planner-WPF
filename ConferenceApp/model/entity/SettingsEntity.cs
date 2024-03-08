namespace ConferenceApp.model.entity;

public class SettingsEntity
{
    public int? UserId { get; set; }
    public string Language { get; set; }
    public string Theme { get; set; }

    public SettingsEntity()
    {
        this.Language = LanguageEnum.DEFAULT.ToString();
        this.Theme = ThemeEnum.DEFAULT.ToString();
    }
}