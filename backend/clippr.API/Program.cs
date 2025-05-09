using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using clippr.API.Authentication;
using clippr.Core.Clip;
using clippr.Core.Repository;
using clippr.Core.User;
using clippr.Repository;
using clippr.Repository.Repositories;
using ILogger = Microsoft.Extensions.Logging.ILogger;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
    {
        o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization header using the Bearer scheme.",
        });
        o.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
    }
);
builder.Services.AddCors();

builder.Services.AddSerilog((configuration) =>
    configuration.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    var config = builder.Configuration.GetSection("Authentication");
    options.Authority = config["Authority"];
    options.Audience = config["ClientId"];

    options.Events = new()
    {
        OnAuthenticationFailed = (context) =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger>();
            logger.LogInformation("Token validation failed. {message}", context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = OnTokenValidatedMiddleware.StoreUser
    };
});

builder.Services.AddDbContext<ClipprDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("Default"), new MariaDbServerVersion(ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default")))));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IClipService, ClipService>();
builder.Services.AddScoped<IRepository<UserModel>, Repository<UserModel>>();
builder.Services.AddScoped<IRepository<ClipModel>, ClipRepository>();
builder.Services.AddScoped<IAuthenticationHelper, AuthenticationHelper>();

builder.Services.AddAuthorization();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapControllers().WithOpenApi();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
