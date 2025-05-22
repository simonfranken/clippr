namespace clippr.IdentityService.Core.JwtKeyProvider;

using Microsoft.IdentityModel.Tokens;

public interface IJwtKeyProviderService
{
    public RsaSecurityKey SecurityKey { get; }
    public JsonWebKey PublicKey { get; }
}
