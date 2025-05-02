using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TgBotGuide.Domain.Entities;

namespace TgBotGuide.Infrastructure.DataBase.Configurations;

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

        builder.Property(l => l.MapUrl)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("mapUrl");

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
    }
}