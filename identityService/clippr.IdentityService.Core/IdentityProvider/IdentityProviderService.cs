using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace clippr.IdentityService.Core.IdentityProvider;

public class IdentityProviderService(IOptions<List<ExternalProvider>> options) : IIdentityProviderService
{
    private readonly List<ExternalProvider> _externalProviders = options.Value;

    public async Task<ValidationResult> Validate(ExternalLoginEvent loginEvent)
    {
        var provider = GetProvider(loginEvent.ProviderKey);
        var validationParameters = await GetValidationParameters(provider);
        var result = await HandleValidation(loginEvent.Token, validationParameters);
        return result;
    }

    private static async Task<TokenValidationParameters> GetValidationParameters(ExternalProvider provider)
    {
        var configUri = new Uri(new Uri(provider.Issuer, UriKind.Absolute), ".well-known/openid-configuration");
        var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(configUri.ToString(), new OpenIdConnectConfigurationRetriever());
        var config = await configManager.GetConfigurationAsync();
        return new TokenValidationParameters()
        {
            ValidIssuer = config.Issuer,
            ValidateIssuer = true,
            ValidAudience = provider.Audience,
            IssuerSigningKeys = config.SigningKeys
        };
    }

    private ExternalProvider GetProvider(string providerKey)
    {
        return _externalProviders.FirstOrDefault(x => x.ProviderKey == providerKey) ?? throw new KeyNotFoundException($"Provider `{providerKey}` does not exist.");
    }

    private static async Task<ValidationResult> HandleValidation(string token, TokenValidationParameters parameters)
    {
        var handler = new JwtSecurityTokenHandler();
        var validationResult = await handler.ValidateTokenAsync(token, parameters);
        if (!validationResult.IsValid)
        {
            return new ValidationResult(false)
            {
                ErrorMessages = [validationResult.Exception.Message]
            };
        }
        return ValidateClaims(validationResult.ClaimsIdentity);
    }

    private static ValidationResult ValidateClaims(ClaimsIdentity claimsIdentity)
    {
        var requiredClaims = new List<string>()
        {
            ClaimTypes.Email,
            ClaimTypes.Surname,
            ClaimTypes.GivenName,
            ClaimTypes.NameIdentifier,
        };
        ValidationResult result = new(false)
        {
            ErrorMessages = [.. requiredClaims.Where(x => claimsIdentity.FindFirst(x) == null).Select(x => $"Claim `{x}` was not found.")]
        };
        if (result.ErrorMessages.Count != 0)
        {
            return result;
        }

        result.Successfull = true;
        var email = claimsIdentity.FindFirst(ClaimTypes.Email)!.Value;
        var familyName = claimsIdentity.FindFirst(ClaimTypes.Surname)!.Value;
        var givenName = claimsIdentity.FindFirst(ClaimTypes.GivenName)!.Value;
        var id = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        UserIdentity userIdentity = new(
            email: email,
            givenName: givenName,
            familyName: familyName,
            id: id
        );
        result.Identity = userIdentity;
        return result;
    }
}