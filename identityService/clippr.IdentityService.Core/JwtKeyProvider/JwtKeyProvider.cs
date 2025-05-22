using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace clippr.IdentityService.Core.JwtKeyProvider;

public class JwtKeyProviderService : IJwtKeyProviderService
{
    private readonly RSA rSA = RSA.Create(2048);
    private readonly RsaSecurityKey rsaSecurityKey;

    public JwtKeyProviderService()
    {
        rsaSecurityKey = new RsaSecurityKey(rSA)
        {
            KeyId = Guid.NewGuid().ToString()
        };
    }

    public RsaSecurityKey SecurityKey => rsaSecurityKey;

    public JsonWebKey PublicKey => JsonWebKeyConverter.ConvertFromRSASecurityKey(rsaSecurityKey);
}
