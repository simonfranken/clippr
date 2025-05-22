using clippr.IdentityService.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace clippr.IdentityService.API;

public class IdentityDbContext : IdentityDbContext<UserModel>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {

    }
}