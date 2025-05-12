using System.Security.Authentication;
using System.Security.Cryptography;
using clippr.Core.AppToken.Specifications;
using clippr.Core.Repository;
using clippr.Core.User;

namespace clippr.Core.AppToken;

public class AppTokenService : IAppTokenService
{
    private const int SecretLength = 32;
    private const int SaltLength = 16;
    private readonly IRepository<AppTokenModel> _repository;

    public AppTokenService(IRepository<AppTokenModel> repository)
    {
        _repository = repository;
    }

    public string CreateToken(UserModel user)
    {
        var secret = GetRandomBytes(SecretLength);
        var appToken = new AppTokenModel(secret, GetRandomBytes(SaltLength), TimeSpan.FromDays(30), user);
        _repository.Add(appToken);
        return GetCombinedTokenString(appToken.Id, secret);
    }

    private static string GetCombinedTokenString(Guid id, byte[] secret)
    {
        return string.Format("{0}.{1}", id.ToString(), Convert.ToBase64String(secret));
    }

    private static byte[] GetRandomBytes(int length)
    {
        using var rng = RandomNumberGenerator.Create();
        byte[] randomBytes = new byte[length];
        rng.GetBytes(randomBytes);

        return randomBytes;
    }

    public UserModel Validate(string token)
    {
        var splittedToken = token.Split(".");
        if (splittedToken.Length != 2)
        {
            throw new InvalidOperationException("Token is in the wrong format.");
        }

        var appToken = _repository.Get(new AppTokenWithUserSpecification()).FirstOrDefault(x => x.Id == Guid.Parse(splittedToken[0])) ?? throw new KeyNotFoundException("Token does not exist.");
        if (appToken.IsExpired)
        {
            throw new AuthenticationException("Token has expired.");
        }

        if (!appToken.Validate(Convert.FromBase64String(splittedToken[1])))
        {
            throw new AuthenticationException("Token is invalid");
        }

        return appToken.User;
    }
}