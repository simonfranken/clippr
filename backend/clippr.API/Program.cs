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
using clippr.API.Background.CleanUp;
using clippr.Core.AppToken;
using clippr.API.Authentication.AppToken;


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
        o.AddServer(new()
        {
            Url = builder.Configuration.GetValue<string>("Hosting:PathBase")
        });
    }
);
builder.Services.AddCors();

builder.Services.AddSerilog((configuration) =>
    configuration.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddAuthentication("MultiScheme")
    .AddPolicyScheme("MultiScheme", "Multiple Auth", options =>
    {
        options.ForwardDefaultSelector = context =>
        {
            var authHeader = context.Request.Headers[AppTokenDefaults.HttpHeaderName].ToString();

            if (authHeader != string.Empty)
            {
                return AppTokenDefaults.AuthenticationScheme;
            }
            return JwtBearerDefaults.AuthenticationScheme;
        };
    })
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration.GetSection("Authentication");
        options.Authority = config["Authority"];
        options.Audience = config["ClientId"];

        options.Events = new()
        {
            OnTokenValidated = OnTokenValidatedMiddleware.StoreUser
        };
    })
    .AddScheme<AppTokenAuthenticationOptions, AppTokenAuthenticationHandler>(AppTokenDefaults.AuthenticationScheme, o => { });

builder.Services.AddDbContext<ClipprDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("Default"), new MariaDbServerVersion(ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("Default")))));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IClipService, ClipService>();
builder.Services.AddScoped<IAppTokenService, AppTokenService>();
builder.Services.AddScoped<IRepository<UserModel>, Repository<UserModel>>();
builder.Services.AddScoped<IRepository<ClipModel>, ClipRepository>();
builder.Services.AddScoped<IRepository<AppTokenModel>, AppTokenRepository>();
builder.Services.AddScoped<IAuthenticationHelper, AuthenticationHelper>();

builder.Services.AddHostedService<CleanupService>();

builder.Services.Configure<CleanupOptions>(builder.Configuration.GetSection("CleanUp"));
builder.Services.Configure<AuthenticationOptions>(builder.Configuration.GetSection("Authentication"));

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
    using var dbContext = scope.ServiceProvider.GetRequiredService<ClipprDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapControllers().WithOpenApi();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.Run();