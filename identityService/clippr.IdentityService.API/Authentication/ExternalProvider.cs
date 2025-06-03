namespace clippr.IdentityService.API.Authentication;

public class ExternalProvider
{
    public string ProviderKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string? PreferredNameClaim { get; set; }
}