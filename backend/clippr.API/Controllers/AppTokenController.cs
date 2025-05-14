
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using clippr.API.Authentication;
using clippr.Core.AppToken;
using clippr.API.DTOs;

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

    [HttpGet]
    public ActionResult GetAll()
    {
        var user = _authHelper.GetUser(User);
        var tokens = _appTokenService.GetAllForUser(user);
        return Ok(tokens.Select(x => new AppTokenDTO(x)));
    }

    [HttpPost]
    public ActionResult CreateAppToken()
    {
        var user = _authHelper.GetUser(User);
        return Ok(_appTokenService.CreateToken(user));
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult DeleteAppToken(Guid id)
    {
        var user = _authHelper.GetUser(User);
        var token = _appTokenService.GetToken(id);
        if (!user.Equals(token.User))
        {
            return Forbid();
        }
        _appTokenService.DeleteToken(id);
        return Ok();
    }
}