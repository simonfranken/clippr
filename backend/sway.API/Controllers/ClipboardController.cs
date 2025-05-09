
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sway.API.Authentication;
using sway.API.DTOs;
using sway.Core.Clip;

namespace sway.API.Controllers;

[Authorize]
[Route("clipboard")]
public class ClipboardController : ControllerBase
{
    private readonly IClipService _clipService;
    private readonly IAuthenticationHelper _authHelper;

    public ClipboardController(IClipService clipService, IAuthenticationHelper authHelper)
    {
        _clipService = clipService;
        _authHelper = authHelper;
    }

    [HttpGet]
    public ActionResult GetAll()
    {
        var user = _authHelper.GetUserWithClips(User);
        return Ok(user.Clips.Select(clip => new ClipDTO(clip)));
    }

    [HttpPost]
    public ActionResult Create(string text)
    {
        var user = _authHelper.GetUser(User);
        var content = new ClipContent(text);
        _clipService.Create(content, user);
        return Ok();
    }
}