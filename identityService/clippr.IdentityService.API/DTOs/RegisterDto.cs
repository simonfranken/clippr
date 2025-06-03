using System.ComponentModel.DataAnnotations;
using clippr.IdentityService.API.Models;
using FluentValidation;

namespace clippr.IdentityService.API.DTOs;

public class RegisterDto
{
    public string? Email { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public string? Password { get; set; }
    public UserModel ToUserModel()
    {
        if (GivenName == null)
        {
            throw new Exception("`GivenName` must be set.");
        }
        if (FamilyName == null)
        {
            throw new Exception("`FamilyName` must be set.");
        }
        if (Email == null)
        {
            throw new Exception("`Email` must be set.");
        }
        return new UserModel()
        {
            Email = Email,
            GivenName = GivenName,
            FamilyName = FamilyName,
            UserName = Email
        };
    }
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