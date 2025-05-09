using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sway.Core.User;

namespace sway.Repository.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder
            .HasKey(x => x.Subject);

        builder
            .HasMany(x => x.Clips)
            .WithOne(x => x.User);
    }
}