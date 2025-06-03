using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using clippr.Core.User;

namespace clippr.API.Authentication;

public class OnTokenValidatedMiddleware
{
    public static Task StoreUser(TokenValidatedContext context)
    {
        var id = context.Principal!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var email = context.Principal!.FindFirst(ClaimTypes.Email)!.Value;
        var givenName = context.Principal!.FindFirst(ClaimTypes.GivenName)!.Value;
        var familyName = context.Principal!.FindFirst(ClaimTypes.Surname)!.Value;

        var _userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

        try
        {
            var user = _userService.GetUser(id);
            if (!IsUserUpToDate(context.Principal, user))
            {
                user.UpdateInformation(givenName, familyName, email);
                _userService.UpdateUser(user);
            }
        }
        catch (KeyNotFoundException)
        {
            _userService.CreateUser(id, givenName, familyName, email);
        }

        return Task.CompletedTask;
    }

    private static bool IsUserUpToDate(ClaimsPrincipal claimsPrincipal, UserModel userModel)
    {
        var givenName = claimsPrincipal.FindFirst(ClaimTypes.GivenName)!.Value;
        var email = claimsPrincipal.FindFirst(ClaimTypes.Email)!.Value;
        var familyName = claimsPrincipal.FindFirst(ClaimTypes.Surname)!.Value;

        return givenName == userModel.GivenName && email == userModel.Email && familyName == userModel.FamilyName;
    }
}
