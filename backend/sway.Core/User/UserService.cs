using sway.Core.Repository;
using sway.Core.User.Specifications;

namespace sway.Core.User;

public class UserService : IUserService
{
    private readonly IRepository<UserModel> _repository;

    public UserService(IRepository<UserModel> repository)
    {
        _repository = repository;
    }

    public void CreateUser(string subject, string givenName, string email)
    {
        var existingUser = _repository.Get().FirstOrDefault(x => x.Subject == subject);
        if (existingUser != null)
        {
            throw new InvalidOperationException("A user with the same subject already exists.");
        }
        var user = new UserModel(subject, givenName, email);
        _repository.Add(user);
    }

    public UserModel GetUser(string subject)
    {
        var user = _repository.Get().FirstOrDefault(x => x.Subject == subject) ?? throw new KeyNotFoundException();
        return user;
    }

    public UserModel GetUserWithClips(string subject)
    {
        var user = _repository.Get(new UserWithClipsSpecification()).FirstOrDefault(x => x.Subject == subject) ?? throw new KeyNotFoundException();
        return user;
    }
}