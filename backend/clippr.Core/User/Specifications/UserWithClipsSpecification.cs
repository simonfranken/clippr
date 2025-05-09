using Ardalis.Specification;

namespace clippr.Core.User.Specifications;

public class UserWithClipsSpecification : Specification<UserModel>
{
    public UserWithClipsSpecification()
    {
        Query
            .Include(x => x.Clips).ThenInclude(x => x.Content);
    }
}