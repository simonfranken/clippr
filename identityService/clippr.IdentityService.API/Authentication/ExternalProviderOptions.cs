namespace clippr.IdentityService.API.Authentication;

public class ExternalProviderOptions
{
    public List<ExternalProvider>? ExternalProviders { get; set; }
    public ExternalProvider? Get(string providerKey)
    {
        return ExternalProviders?.Find(x => x.ProviderKey == providerKey);
    }
}