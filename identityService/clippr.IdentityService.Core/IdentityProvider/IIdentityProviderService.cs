namespace clippr.IdentityService.Core.IdentityProvider;

public interface IIdentityProviderService
{
    public Task<ValidationResult> Validate(ExternalLoginEvent loginEvent);
}