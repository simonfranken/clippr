namespace clippr.IdentityService.API.DTOs;

public class ExternalLoginDto
{
    public string IdToken { get; set; }
    public string ProviderKey { get; set; }

    public ExternalLoginDto(string idToken, string providerKey)
    {
        IdToken = idToken;
        ProviderKey = providerKey;
    }
}