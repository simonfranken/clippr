namespace clippr.API.Background.CleanUp;

public class CleanupOptions
{
    public string CronExpression { get; set; } = "0 * * * *";
    public bool Enabled { get; set; }
    public int MaxClipAgeHours { get; set; }
}