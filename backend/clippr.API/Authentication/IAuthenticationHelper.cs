using System.Security.Claims;
using clippr.Core.User;

namespace clippr.API.Authentication;

public interface IAuthenticationHelper
{
    public UserModel GetUser(ClaimsPrincipal principal);
    public UserModel GetUserWithClips(ClaimsPrincipal principal);
}