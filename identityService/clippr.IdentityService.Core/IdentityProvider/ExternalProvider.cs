namespace clippr.IdentityService.Core.IdentityProvider;

public class ExternalProvider
{
    public ExternalProvider(string providerKey, string issuer, string audience)
    {
        ProviderKey = providerKey;
        Issuer = issuer;
        Audience = audience;
    }

    public string ProviderKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}