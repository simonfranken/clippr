using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using clippr.IdentityService.API.DTOs;
using clippr.IdentityService.API.Models;
using clippr.IdentityService.Core.IdentityProvider;
using clippr.IdentityService.Core.JwtKeyProvider;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace clippr.IdentityService.API.Controllers;

[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<UserModel> _userManager;
    private readonly IConfiguration _config;
    private readonly IJwtKeyProviderService _jwtKeyProfider;
    private readonly RegisterDtoValidator _registerDtoValidator;
    private readonly LoginDtoValidator _loginDtoValidator;
    private readonly IIdentityProviderService _identityProviderService;

    public AuthController(UserManager<UserModel> userManager,
                          IConfiguration config,
                          IJwtKeyProviderService jwtKeyProfider,
                          RegisterDtoValidator registerDtoValidator,
                          LoginDtoValidator loginDtoValidator,
                          IIdentityProviderService identityProviderService)
    {
        _userManager = userManager;
        _config = config;
        _jwtKeyProfider = jwtKeyProfider;
        _registerDtoValidator = registerDtoValidator;
        _loginDtoValidator = loginDtoValidator;
        _identityProviderService = identityProviderService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!_registerDtoValidator.Validate(dto).IsValid)
        {
            return BadRequest();
        }

        var existingUser = await _userManager.FindByEmailAsync(dto.Email!);
        if (existingUser != null)
        {
            return BadRequest("User with email already exists.");
        }

        var user = new UserModel(
            email: dto.Email!,
            familyName: dto.FamilyName!,
            givenName: dto.GivenName!
        );

        var result = await _userManager.CreateAsync(user, dto.Password!);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { Token = GenerateJwtToken(user) });
    }

    [HttpPost("external-login")]
    public async Task<IActionResult> ExternalLogin([FromBody] ExternalLoginEvent dto)
    {
        var validationResult = await _identityProviderService.Validate(dto);

        if (!validationResult.Successfull)
        {
            return Unauthorized(validationResult.ErrorMessages);
        }

        var linkedUser = await _userManager.FindByLoginAsync(dto.ProviderKey, validationResult.Identity!.Id);
        if (linkedUser != null)
        {
            return Ok(GenerateJwtToken(linkedUser));
        }

        var existingUser = await _userManager.FindByEmailAsync(validationResult.Identity.Email);
        if (existingUser != null)
        {
            return Unauthorized($"User with email `{validationResult.Identity.Email}` already exists, but is not linked.");
        }

        var newUser = new UserModel(
            email: validationResult.Identity!.Email,
            givenName: validationResult.Identity!.GivenName,
            familyName: validationResult.Identity!.FamilyName
        );

        await _userManager.CreateAsync(newUser);
        await _userManager.AddLoginAsync(newUser, new(dto.ProviderKey, validationResult.Identity.Id, dto.ProviderKey));
        return Ok(new { Token = GenerateJwtToken(newUser) });
    }

    [HttpPost("link-external-login")]
    public async Task<IActionResult> LinkExternalLogin([FromBody] LinkExternalLoginDto dto)
    {
        if (!_loginDtoValidator.Validate(dto.LoginDto).IsValid)
        {
            return BadRequest();
        }
        var user = await _userManager.FindByEmailAsync(dto.LoginDto.Email!);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.LoginDto.Password!))
        {
            return Unauthorized();
        }

        var validationResult = await _identityProviderService.Validate(dto.ExternalLoginEvent);
        if (!validationResult.Successfull)
        {
            return Unauthorized();
        }

        var linkedUser = await _userManager.FindByLoginAsync(dto.ExternalLoginEvent.ProviderKey, validationResult.Identity!.Id);
        if (linkedUser != null)
        {
            return BadRequest("Account already linked.");
        }

        await _userManager.AddLoginAsync(user, new(dto.ExternalLoginEvent.ProviderKey, validationResult.Identity.Id, dto.ExternalLoginEvent.ProviderKey));
        return Ok(new { Token = GenerateJwtToken(user) });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!_loginDtoValidator.Validate(dto).IsValid)
        {
            return BadRequest();
        }
        var user = await _userManager.FindByEmailAsync(dto.Email!);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password!))
        {
            return Unauthorized();
        }

        var token = GenerateJwtToken(user);
        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(UserModel user)
    {
        var jwtSettings = _config.GetSection("JwtSettings");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.GivenName, user.GivenName!),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.FamilyName!),
        };

        var creds = new SigningCredentials(_jwtKeyProfider.SecurityKey, SecurityAlgorithms.RsaSha256);

        var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiresInMinutes"]!));

        var origin = _config.GetValue<string>("Hosting:Url");

        var token = new JwtSecurityToken(
            issuer: origin,
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}