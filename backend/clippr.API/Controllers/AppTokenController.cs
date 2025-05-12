
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using clippr.API.Authentication;
using clippr.Core.AppToken;

namespace clippr.API.Controllers;

[Authorize]
[Route("apptoken")]
public class AppTokenController : ControllerBase
{
    private readonly IAppTokenService _appTokenService;
    private readonly IAuthenticationHelper _authHelper;

    public AppTokenController(IAppTokenService appTokenService, IAuthenticationHelper authHelper)
    {
        _appTokenService = appTokenService;
        _authHelper = authHelper;
    }

    [HttpPost]
    public ActionResult CreateAppToken()
    {
        var user = _authHelper.GetUser(User);
        return Ok(_appTokenService.CreateToken(user));
    }
}