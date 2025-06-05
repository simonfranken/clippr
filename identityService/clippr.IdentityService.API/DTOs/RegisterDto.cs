using FluentValidation;

namespace clippr.IdentityService.API.DTOs;

public class RegisterDto
{
    public string? Email { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public string? Password { get; set; }
}

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => new { x.GivenName, x.FamilyName, x.Password })
            .NotEmpty();
    }
}