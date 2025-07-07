using clippr.API.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace clippr.API.Controllers;

[Route("configuration")]
public class ConfigurationController : ControllerBase
{
    private readonly IdentityServiceOptions _authenticationOptions;

    public ConfigurationController(IOptions<IdentityServiceOptions> authenticationOptions)
    {
        _authenticationOptions = authenticationOptions.Value;
    }

    [HttpGet]
    [Route("idp")]
    public ActionResult GetIdpConfiguration()
    {
        return Ok(new
        {
            Issuer = _authenticationOptions.IssuerPublicUrl,
            _authenticationOptions.Audience
        });
    }
}