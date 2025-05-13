using clippr.Core.AppToken;

namespace clippr.API.DTOs;

public class AppTokenDTO
{
    public AppTokenDTO(AppTokenModel appToken)
    {
        Id = appToken.Id;
        CreatedAt = appToken.CreatedAt;
        ExpirationDate = appToken.ExpirationDate;

    }
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
}