using clippr.Core.AppToken;
using clippr.Core.Clip;

namespace clippr.Core.User;

public class UserModel
{
    public UserModel(string id, string givenName, string email, string familyName)
    {
        Id = id;
        GivenName = givenName;
        Email = email;
        FamilyName = familyName;
    }

    public string Id { get; set; }
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public string Email { get; set; }
    public virtual List<ClipModel> Clips { get; set; } = new List<ClipModel>();
    public virtual List<AppTokenModel> AppTokens { get; set; } = new List<AppTokenModel>();
    public void UpdateInformation(string givenName, string familyName, string email)
    {
        GivenName = givenName;
        FamilyName = familyName;
        Email = email;
    }

    public override bool Equals(object? obj)
    {
        if (obj is UserModel user)
        {
            return Id == user.Id;
        }
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
