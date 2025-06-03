using System.Security.Claims;
using System.Text.Encodings.Web;
using clippr.Core.AppToken;
using clippr.Core.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace clippr.API.Authentication.AppToken;

public class AppTokenAuthenticationHandler : AuthenticationHandler<AppTokenAuthenticationOptions>
{
    private readonly IAppTokenService _appTokenService;

    [Obsolete]
    public AppTokenAuthenticationHandler(IOptionsMonitor<AppTokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IAppTokenService appTokenService) : base(options, logger, encoder, clock)
    {
        _appTokenService = appTokenService;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var token = Request.Headers[AppTokenDefaults.HttpHeaderName].ToString();
        if (token == null)
        {
            return Task.FromResult(AuthenticateResult.Fail("No token provided."));
        }

        UserModel user;
        try
        {
            user = _appTokenService.Validate(token);
        }
        catch (Exception e)
        {
            return Task.FromResult(AuthenticateResult.Fail(e.Message));
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.GivenName, user.GivenName),
            new(ClaimTypes.Email, user.Email),
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}