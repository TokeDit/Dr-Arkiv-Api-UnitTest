namespace DR.Arkiv.API.Models;

public class MusicRecord
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    /// <summary>Duration in seconds</summary>
    public int Duration { get; set; }
    public int PublicationYear { get; set; }
}
