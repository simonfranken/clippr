namespace clippr.API.Authentication;

public class IdentityServiceOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}