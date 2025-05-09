using clippr.Core.Clip;

namespace clippr.API.DTOs;

public class ClipDTO
{
    public ClipDTO(ClipModel clip)
    {
        Id = clip.Id;
        Type = Enum.GetName(clip.Content.Type)!;
        Base64Data = Convert.ToBase64String(clip.Content.Data);
        CreatedAt = clip.CreatedAt;
    }

    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Base64Data { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}