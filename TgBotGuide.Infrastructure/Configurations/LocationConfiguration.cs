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
            .IsRequired(false)
            .HasColumnName("description");

        builder.Property(l => l.Coordinates)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("coordinates");

        builder.OwnsOne(l => l.Address, address =>
        {
            address.Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("street"); 

            address.Property(a => a.House)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("house"); 
        });

        builder.Property(l => l.ImageUrl)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnName("image_url");
        
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