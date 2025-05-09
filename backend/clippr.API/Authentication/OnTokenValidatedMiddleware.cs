using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using clippr.Core.User;

namespace clippr.API.Authentication;

public class OnTokenValidatedMiddleware
{
    public static Task StoreUser(TokenValidatedContext context)
    {
        var subject = context.Principal!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var givenName = context.Principal!.FindFirst(ClaimTypes.GivenName)!.Value;
        var email = context.Principal!.FindFirst(ClaimTypes.Email)!.Value;
        var _userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

        try
        {
            var user = _userService.GetUser(subject);
            if (!IsUserUpToDate(context.Principal, user))
            {
                user.UpdateInformation(givenName, email);
                _userService.UpdateUser(user);
            }
        }
        catch (KeyNotFoundException)
        {
            _userService.CreateUser(subject, givenName, email);
        }

        return Task.CompletedTask;
    }

    private static bool IsUserUpToDate(ClaimsPrincipal claimsPrincipal, UserModel userModel)
    {
        var givenName = claimsPrincipal.FindFirst(ClaimTypes.GivenName)!.Value;
        var email = claimsPrincipal.FindFirst(ClaimTypes.Email)!.Value;

        return givenName == userModel.GivenName && email == userModel.Email;
    }
}
