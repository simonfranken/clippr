using Microsoft.AspNetCore.Identity;

namespace clippr.IdentityService.API.Models;

public class UserModel : IdentityUser
{
    public string GivenName { get; set; }
    public string FamilyName { get; set; }
}