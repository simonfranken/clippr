namespace clippr.IdentityService.Core.IdentityProvider;

public class ExternalLoginEvent
{
    public string Token { get; set; }
    public string ProviderKey { get; set; }

    public ExternalLoginEvent(string idToken, string providerKey)
    {
        Token = idToken;
        ProviderKey = providerKey;
    }
}