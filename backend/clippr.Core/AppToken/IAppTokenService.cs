using clippr.Core.User;

namespace clippr.Core.AppToken;

public interface IAppTokenService
{
    public string CreateToken(UserModel user);
    public void DeleteToken(Guid id);
    public AppTokenModel GetToken(Guid id);
    public UserModel Validate(string token);
    public List<AppTokenModel> GetAllForUser(UserModel user);
}