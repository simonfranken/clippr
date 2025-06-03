using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using clippr.IdentityService.API.Authentication;
using clippr.IdentityService.API.DTOs;
using clippr.IdentityService.API.Models;
using clippr.IdentityService.Core.JwtKeyProvider;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace clippr.IdentityService.API.Controllers;

[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<UserModel> _userManager;
    private readonly IConfiguration _config;
    private readonly IJwtKeyProviderService _jwtKeyProfider;
    private readonly ExternalProviderOptions _externalProviderOptions;
    private readonly RegisterDtoValidator _registerDtoValidator;
    private readonly LoginDtoValidator _loginDtoValidator;
    public AuthController(UserManager<UserModel> userManager, IConfiguration config, IJwtKeyProviderService jwtKeyProfider, IOptions<ExternalProviderOptions> options, RegisterDtoValidator registerDtoValidator, LoginDtoValidator loginDtoValidator)
    {
        _userManager = userManager;
        _config = config;
        _jwtKeyProfider = jwtKeyProfider;
        _externalProviderOptions = options.Value;
        _registerDtoValidator = registerDtoValidator;
        _loginDtoValidator = loginDtoValidator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!_registerDtoValidator.Validate(dto).IsValid)
        {
            return BadRequest();
        }

        var user = dto.ToUserModel();
        var result = await _userManager.CreateAsync(user, dto.Password!);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { Message = "User created successfully" });
    }

    [HttpPost("external-login")]
    public async Task<IActionResult> ExternalLogin([FromBody] ExternalLoginDto dto)
    {
        var provider = _externalProviderOptions.Get(dto.ProviderKey);
        if (provider == null)
        {
            return BadRequest("Provider does not exist.");
        }

        var handler = new JwtSecurityTokenHandler();
        var configManager = new ConfigurationManager<OpenIdConnectConfiguration>($"{provider.Issuer}/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever());
        var config = await configManager.GetConfigurationAsync();
        var parameters = new TokenValidationParameters()
        {
            ValidIssuer = config.Issuer,
            ValidateIssuer = true,
            ValidAudience = provider.Audience,
            IssuerSigningKeys = config.SigningKeys
        };

        ClaimsPrincipal principal;
        RegisterDto registerDto;
        try
        {
            principal = handler.ValidateToken(dto.IdToken, parameters, out _);
            registerDto = new()
            {
                Email = principal.FindFirstValue(ClaimTypes.Email),
                FamilyName = principal.FindFirstValue(ClaimTypes.Surname),
                GivenName = principal.FindFirstValue(ClaimTypes.GivenName),
            };
        }
        catch
        {
            return BadRequest("Token validation failed.");
        }

        if (!_registerDtoValidator.Validate(registerDto).IsValid)
        {
            return BadRequest();
        }

        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email!);

        if (existingUser != null)
        {
            return Ok(new { Token = GenerateJwtToken(existingUser) });
        }

        var createdUser = registerDto.ToUserModel();
        await _userManager.CreateAsync(createdUser);

        return Ok(new { Token = GenerateJwtToken(createdUser) });
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