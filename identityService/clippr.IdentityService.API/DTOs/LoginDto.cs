using FluentValidation;

namespace clippr.IdentityService.API.DTOs;

public class LoginDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => new { x.Email, x.Password })
            .NotEmpty();
    }
}