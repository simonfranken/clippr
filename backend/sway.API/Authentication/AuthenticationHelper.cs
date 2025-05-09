using System.Security.Claims;
using sway.Core.User;

namespace sway.API.Authentication;

public class AuthenticationHelper : IAuthenticationHelper
{
    private readonly IUserService _userService;

    public AuthenticationHelper(IUserService userService)
    {
        _userService = userService;
    }

    public UserModel GetUser(ClaimsPrincipal principal)
    {
        return _userService.GetUser(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    public UserModel GetUserWithClips(ClaimsPrincipal principal)
    {
        return _userService.GetUserWithClips(principal.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}