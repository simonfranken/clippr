using clippr.IdentityService.Core.IdentityProvider;

namespace clippr.IdentityService.API.DTOs;

public class LinkExternalLoginDto
{
    public LinkExternalLoginDto(LoginDto loginDto, ExternalLoginEvent externalLoginEvent)
    {
        LoginDto = loginDto;
        ExternalLoginEvent = externalLoginEvent;
    }

    public LoginDto LoginDto { get; set; }
    public ExternalLoginEvent ExternalLoginEvent { get; set; }
}