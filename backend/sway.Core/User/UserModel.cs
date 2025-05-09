using sway.Core.Clip;

namespace sway.Core.User;

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
}
