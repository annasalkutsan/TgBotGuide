using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TgBotGuide.Domain.Entities;

namespace TgBotGuide.Infrastructure.Configurations;

public class LocationCategoryConfiguration : IEntityTypeConfiguration<LocationCategory>
{
    public void Configure(EntityTypeBuilder<LocationCategory> builder)
    {
        builder.ToTable("locations_categories");

        builder.HasKey(lc => new { lc.LocationId, lc.CategoryId });

        builder.Property(lc => lc.LocationId)
            .HasColumnName("location_id");

        builder.Property(lc => lc.CategoryId)
            .HasColumnName("category_id");

        // с Location (N:1)
        builder.HasOne(lc => lc.Location)
            .WithMany(l => l.LocationsCategories)
            .HasForeignKey(lc => lc.LocationId)
            .OnDelete(DeleteBehavior.Cascade);

        // с Category (N:1)
        builder.HasOne(lc => lc.Category)
            .WithMany(c => c.LocationsCategories)
            .HasForeignKey(lc => lc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}