using clippr.Core.Repository;
using clippr.Core.User.Specifications;

namespace clippr.Core.User;

public class UserService : IUserService
{
    private readonly IRepository<UserModel> _repository;

    public UserService(IRepository<UserModel> repository)
    {
        _repository = repository;
    }

    public void CreateUser(string id, string givenName, string familyName, string email)
    {
        var existingUser = _repository.Get().FirstOrDefault(x => x.Id == id);
        if (existingUser != null)
        {
            throw new InvalidOperationException("A user with the same id already exists.");
        }
        var user = new UserModel(
            id: id,
            givenName: givenName,
            familyName: familyName,
            email: email
        );
        _repository.Add(user);
    }

    public UserModel GetUser(string id)
    {
        var user = _repository.Get().FirstOrDefault(x => x.Id == id) ?? throw new KeyNotFoundException();
        return user;
    }

    public UserModel GetUserWithClips(string subject)
    {
        var user = _repository.Get(new UserWithClipsSpecification()).FirstOrDefault(x => x.Id == subject) ?? throw new KeyNotFoundException();
        return user;
    }

    public void UpdateUser(UserModel userModel)
    {
        _repository.Update(userModel);
    }
}