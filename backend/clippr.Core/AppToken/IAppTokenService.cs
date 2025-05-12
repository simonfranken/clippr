using clippr.Core.User;

namespace clippr.Core.AppToken;

public interface IAppTokenService
{
    public string CreateToken(UserModel user);
    public UserModel Validate(string token);
}