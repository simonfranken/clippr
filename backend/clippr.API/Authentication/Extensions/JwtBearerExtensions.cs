using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace clippr.API.Authentication.Extensions;

public static class JwtBearerExtensions
{
    public static AuthenticationBuilder AddIdentityService(this AuthenticationBuilder builder, IConfiguration config)
    {
        var identityServiceOptions = config.GetSection("Authentication:IdentityService");

        var issuer = identityServiceOptions["Issuer"] ?? throw new Exception("`Issuer` is not configured.");
        var audience = identityServiceOptions["Audience"] ?? throw new Exception("`Audience` is not configured.");

        builder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKeyResolver = GetSigningKeys,
                ValidateIssuer = true,
                ValidateAudience = true,
            };

            options.Events = new()
            {
                OnTokenValidated = OnTokenValidatedMiddleware.StoreUser
            };
        });

        return builder;
    }

    private static IEnumerable<SecurityKey> GetSigningKeys(string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters)
    {
        var issuer = validationParameters.ValidIssuer;
        using var httpClient = new HttpClient();
        JObject jwks = JObject.Parse(httpClient.GetStringAsync($"{issuer}/.well-known/jwks").GetAwaiter().GetResult());

        if (jwks["keys"] == null)
        {
            throw new Exception("Endpoint does not contain any keys.");
        }
        var keys = jwks.GetValue("keys")!.Where(x => x["kty"]?.ToString() == "RSA").Select(x =>
        {
            if (x["e"] == null || x["n"] == null || x["kid"] == null)
            {
                throw new Exception("Key is missing some data.");
            }
            var e = Base64UrlEncoder.DecodeBytes(x["e"]!.ToString());
            var n = Base64UrlEncoder.DecodeBytes(x["n"]!.ToString());
            var rsa = RSA.Create();
            rsa.ImportParameters(new RSAParameters { Exponent = e, Modulus = n });
            return new RsaSecurityKey(rsa) { KeyId = x["kid"]!.ToString() };
        });

        return keys;
    }
}