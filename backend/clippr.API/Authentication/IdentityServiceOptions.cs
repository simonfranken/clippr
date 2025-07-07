namespace clippr.API.Authentication;

public class IdentityServiceOptions
{
    public string IssuerPublicUrl { get; set; } = string.Empty;
    public string IssuerInternalUrl { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}