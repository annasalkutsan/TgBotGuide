using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TgBotGuide.Domain.Entities;

namespace TgBotGuide.Infrastructure.Configurations;

public class LocationConfiguration: IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id)
            .HasColumnName("id");

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        builder.Property(l => l.Description)
            .HasMaxLength(500)
            .HasColumnName("description");

        builder.Property(l => l.Coordinates)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("coordinates");

        builder.Property(l => l.Address)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("address");
        
        builder.Property(l => l.CityId)
            .HasColumnName("city_id");

        builder.Property(l => l.CreationDate)
            .HasColumnName("creation_date");

        // с Cities (1:N)
        builder.HasOne(l => l.City)
            .WithMany(c => c.Locations)
            .HasForeignKey(l => l.CityId)
            .OnDelete(DeleteBehavior.Cascade);

        // с LocationCategory (1:N)
        builder.HasMany(l => l.LocationsCategories)
            .WithOne(lc => lc.Location)
            .HasForeignKey(lc => lc.LocationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}