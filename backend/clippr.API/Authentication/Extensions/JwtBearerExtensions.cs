using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace clippr.API.Authentication.Extensions;

public static class JwtBearerExtensions
{
    public static AuthenticationBuilder AddIdentityService(this AuthenticationBuilder builder, IConfiguration config)
    {
        var identityServiceOptions = config.GetSection("Authentication:IdentityService").Get<IdentityServiceOptions>()!;

        builder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidIssuer = identityServiceOptions.IssuerPublicUrl,
                ValidAudience = identityServiceOptions.Audience,
                IssuerSigningKeyResolver = GetSigningKeys(identityServiceOptions),
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

    private static IssuerSigningKeyResolver GetSigningKeys(IdentityServiceOptions options)
    {
        return (token, securityToken, kid, validationParameters) =>
        {
            var issuer = options.IssuerInternalUrl;
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
        };
    }
}