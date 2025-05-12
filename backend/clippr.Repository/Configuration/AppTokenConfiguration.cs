
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using clippr.Core.AppToken;

namespace clippr.Repository.Configuration;

public class AppTokenConfiguration : IEntityTypeConfiguration<AppTokenModel>
{
    public void Configure(EntityTypeBuilder<AppTokenModel> builder)
    {
        builder
            .HasKey(x => x.Id);
    }
}