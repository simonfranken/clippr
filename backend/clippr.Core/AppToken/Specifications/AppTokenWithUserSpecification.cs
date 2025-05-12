using Ardalis.Specification;

namespace clippr.Core.AppToken.Specifications;

public class AppTokenWithUserSpecification : Specification<AppTokenModel>
{
    public AppTokenWithUserSpecification()
    {
        Query
            .Include(x => x.User);
    }
}