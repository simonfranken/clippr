using System.Security.Claims;
using sway.Core.User;

namespace sway.API.Authentication;

public interface IAuthenticationHelper
{
    public UserModel GetUser(ClaimsPrincipal principal);
    public UserModel GetUserWithClips(ClaimsPrincipal principal);
}