using Microsoft.EntityFrameworkCore;
using sway.Core.Clip;
using sway.Core.User;
using sway.Repository.Configuration;

namespace sway.Repository;

public class SwayDbContext : DbContext
{
    public SwayDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<UserModel> Users { get; set; }
    public DbSet<ClipModel> Clips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new ClipConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
