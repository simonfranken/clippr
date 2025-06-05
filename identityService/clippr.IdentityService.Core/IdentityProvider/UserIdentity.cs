namespace clippr.IdentityService.Core.IdentityProvider;

public class UserIdentity
{
    public UserIdentity(string email, string givenName, string familyName, string id)
    {
        Email = email;
        GivenName = givenName;
        FamilyName = familyName;
        Id = id;
    }

    public string Email { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public string Id { get; set; }
}