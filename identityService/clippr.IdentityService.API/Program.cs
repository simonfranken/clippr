using clippr.IdentityService.API;
using clippr.IdentityService.API.Models;
using clippr.IdentityService.Core.JwtKeyProvider;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddServer(new()
    {
        Url = builder.Configuration.GetValue<string>("Hosting:Url")
    });
});

builder.Services.AddDbContext<IdentityDbContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("Default"), new MariaDbServerVersion(ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default")))));

builder.Services.AddIdentity<UserModel, IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IJwtKeyProviderService, JwtKeyProviderService>();

builder.Services.AddSerilog((configuration) =>
    configuration.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    using var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    dbContext.Database.Migrate();
}

app.MapGet("/.well-known/jwks", (IJwtKeyProviderService jwtProvider) =>
{
    var jwk = jwtProvider.PublicKey;
    return Results.Ok(new { keys = new[] { jwk } });
});

app.MapGet("/.well-known/openid-configuration", (HttpContext ctx) =>
{
    var origin = ctx.RequestServices.GetRequiredService<IConfiguration>().GetValue<string>("Hosting:Url");
    return Results.Json(new
    {
        issuer = origin,
        jwks_uri = $"{origin}/.well-known/jwks",
        authorization_endpoint = $"{origin}/api/auth/authorize",
        token_endpoint = $"{origin}/api/auth/token",
        userinfo_endpoint = $"{origin}/api/auth/userinfo",
        response_types_supported = new[] { "token", "id_token", "code" },
        subject_types_supported = new[] { "public" },
        id_token_signing_alg_values_supported = new[] { SecurityAlgorithms.RsaSha256 }
    });
});

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.Run();