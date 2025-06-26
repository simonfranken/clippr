using clippr.IdentityService.Core.IdentityProvider;
using FluentValidation;

namespace clippr.IdentityService.API.DTOs;

public class LinkExternalLoginDto
{
    public string? Token { get; set; }
    public string? ProviderKey { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class LinkExternalLoginDtoValidator : AbstractValidator<LinkExternalLoginDto>
{
    public LinkExternalLoginDtoValidator()
    {
        RuleFor(x => new { x.Email, x.Password, x.Token, x.ProviderKey })
            .NotEmpty();
    }
}