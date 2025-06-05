using Microsoft.AspNetCore.Identity;

namespace clippr.IdentityService.API.Models;

public class UserModel : IdentityUser
{
    public UserModel(string givenName, string familyName, string email)
    {
        GivenName = givenName;
        FamilyName = familyName;
        Email = email;
        UserName = email;
    }

    public string GivenName { get; set; }
    public string FamilyName { get; set; }
    public new string Email { get => base.Email!; set => base.Email = value; }
}