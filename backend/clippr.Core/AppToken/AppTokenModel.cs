using System.Security.Cryptography;
using clippr.Core.User;

namespace clippr.Core.AppToken;

public class AppTokenModel
{
    public Guid Id { get; set; }

    public AppTokenModel(Guid id, DateTimeOffset expirationDate, byte[] salt, byte[] hash)
    {
        Id = id;
        ExpirationDate = expirationDate;
        Salt = salt;
        Hash = hash;
    }

    public AppTokenModel(byte[] secret, byte[] salt, TimeSpan lifetime, UserModel userModel)
    {
        Id = Guid.NewGuid();
        ExpirationDate = DateTimeOffset.Now + lifetime;
        User = userModel;
        Salt = salt;

        using var rfc2898 = new Rfc2898DeriveBytes(secret, salt, 100_000, HashAlgorithmName.SHA256);
        Hash = rfc2898.GetBytes(32);
    }

    public bool Validate(byte[] secret)
    {
        using var rfc2898 = new Rfc2898DeriveBytes(secret, Salt, 100_000, HashAlgorithmName.SHA256);
        var inputHash = rfc2898.GetBytes(32);
        return Hash.SequenceEqual(inputHash);
    }

    public virtual UserModel User { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public byte[] Salt { get; set; }
    public byte[] Hash { get; set; }
    public bool IsExpired => ExpirationDate <= DateTimeOffset.Now;
}