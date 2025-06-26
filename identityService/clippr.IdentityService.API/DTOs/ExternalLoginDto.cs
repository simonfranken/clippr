using FluentValidation;

namespace clippr.IdentityService.API.DTOs;

public class ExternalLoginDto
{
    public string? Token { get; set; }
    public string? ProviderKey { get; set; }
}

public class ExternalLoginDtoValidator : AbstractValidator<ExternalLoginDto>
{
    public ExternalLoginDtoValidator()
    {
        RuleFor(x => new { x.Token, x.ProviderKey })
            .NotEmpty();
    }
}