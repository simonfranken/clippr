using clippr.API.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace clippr.API.Controllers;

[Route("configuration")]
public class ConfigurationController : ControllerBase
{
    private readonly AuthenticationOptions _authenticationOptions;

    public ConfigurationController(IOptions<AuthenticationOptions> authenticationOptions)
    {
        _authenticationOptions = authenticationOptions.Value;
    }

    [HttpGet]
    [Route("idp")]
    public ActionResult GetIdpConfiguration()
    {
        return Ok(_authenticationOptions);
    }
}