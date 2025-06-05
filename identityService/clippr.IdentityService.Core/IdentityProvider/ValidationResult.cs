namespace clippr.IdentityService.Core.IdentityProvider;

public class ValidationResult
{
    public ValidationResult(bool successfull, UserIdentity? identity = null)
    {
        Successfull = successfull;
        Identity = identity;
    }

    public bool Successfull { get; set; }
    public List<string> ErrorMessages { get; set; } = [];
    public UserIdentity? Identity { get; set; }
}