public class SettingsData
{
    public bool FirstStartGame { get; set; }
    public bool FullScreen { get; set; }
    public ResItem ResItem { get; set; }
    public float EffectsVolume { get; set; }
    public float MusicVolume { get; set; }
    public int Language { get; set; }

    public static Newtonsoft.Json.JsonSerializerSettings SerializeSettings()
    {
        return new Newtonsoft.Json.JsonSerializerSettings
        {
            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        };
    }
}
