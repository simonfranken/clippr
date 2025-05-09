namespace sway.Core.User;

public interface IUserService
{
    public UserModel GetUser(string subject);
    public UserModel GetUserWithClips(string subject);
    public void CreateUser(string subject, string givenName, string email);
}