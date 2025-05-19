
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using clippr.API.Authentication;
using clippr.API.DTOs;
using clippr.Core.Clip;
using Microsoft.Extensions.Options;

namespace clippr.API.Controllers;

[Authorize]
[Route("clipboard")]
public class ClipboardController : ControllerBase
{
    private readonly IClipService _clipService;
    private readonly ClipOptions _clipOptions;
    private readonly IAuthenticationHelper _authHelper;

    public ClipboardController(IClipService clipService, IAuthenticationHelper authHelper, IOptions<ClipOptions> options)
    {
        _clipService = clipService;
        _authHelper = authHelper;
        _clipOptions = options.Value;
    }

    [HttpGet]
    public ActionResult GetAll()
    {
        var user = _authHelper.GetUserWithClips(User);
        return Ok(user.Clips.Select(clip => new ClipDTO(clip)));
    }

    [HttpPost]
    public ActionResult Create([FromBody] string text)
    {
        var user = _authHelper.GetUser(User);
        var content = new ClipContent(text);
        _clipService.Create(content, user);
        return Ok();
    }

    [HttpPost]
    [Route("file")]
    public ActionResult Create(IFormFile file)
    {
        var user = _authHelper.GetUser(User);
        using var memoryStream = new MemoryStream();
        file.CopyTo(memoryStream);
        var content = new ClipContent(memoryStream.ToArray(), ClipContentType.File);
        _clipService.Create(content, user);
        return Ok();
    }
}