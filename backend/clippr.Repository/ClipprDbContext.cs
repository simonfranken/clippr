using Microsoft.EntityFrameworkCore;
using clippr.Core.Clip;
using clippr.Core.User;
using clippr.Repository.Configuration;

namespace clippr.Repository;

public class ClipprDbContext : DbContext
{
    public ClipprDbContext(DbContextOptions options) : base(options)
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
