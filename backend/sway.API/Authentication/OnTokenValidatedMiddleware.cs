using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using sway.Core.User;

namespace sway.API.Authentication;

public class OnTokenValidatedMiddleware
{
    public static Task StoreUser(TokenValidatedContext context)
    {
        var subject = context.Principal!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var _userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();

        try
        {
            var user = _userService.GetUser(subject);
        }
        catch (KeyNotFoundException)
        {
            var givenName = context.Principal!.FindFirst(ClaimTypes.GivenName)!.Value;
            var email = context.Principal!.FindFirst(ClaimTypes.Email)!.Value;
            _userService.CreateUser(subject, givenName, email);
        }

        return Task.CompletedTask;
    }
}