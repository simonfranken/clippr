using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sway.Core.Clip;

namespace sway.Repository.Configuration;

public class ClipConfiguration : IEntityTypeConfiguration<ClipModel>
{
    public void Configure(EntityTypeBuilder<ClipModel> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .OwnsOne(x => x.Content)
            .WithOwner();
    }
}