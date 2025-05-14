using clippr.Core.AppToken;
using clippr.Core.Clip;

namespace clippr.Core.User;

public class UserModel
{
    public UserModel(string subject, string givenName, string email)
    {
        Subject = subject;
        GivenName = givenName;
        Email = email;
    }

    public string Subject { get; set; }
    public string GivenName { get; set; }
    public string Email { get; set; }
    public virtual List<ClipModel> Clips { get; set; } = new List<ClipModel>();
    public virtual List<AppTokenModel> AppTokens { get; set; } = new List<AppTokenModel>();
    public void UpdateInformation(string givenName, string email)
    {
        GivenName = givenName;
        Email = email;
    }

    public override bool Equals(object? obj)
    {
        if (obj is UserModel user)
        {
            return Subject == user.Subject;
        }
        return base.Equals(obj);
    }
}
