using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using clippr.Core.User;

namespace clippr.Repository.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .HasMany(x => x.Clips)
            .WithOne(x => x.User);

        builder
            .HasMany(x => x.AppTokens)
            .WithOne(x => x.User);
    }
}