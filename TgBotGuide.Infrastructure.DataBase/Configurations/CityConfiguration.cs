using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TgBotGuide.Domain.Entities;

namespace TgBotGuide.Infrastructure.DataBase.Configurations;

public class CityConfiguration: IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("cities");

        builder.HasKey(c => c.Id);
        builder.Property(c=>c.Id)
            .HasColumnName("id");
        
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");
        
        builder.Property(c => c.Description)
            .HasMaxLength(500)
            .IsRequired(false)
            .HasColumnName("description");
        
        builder.Property(c=>c.CreationDate)
            .HasColumnName("creation_date");

        // 1:N между City и Locations
        builder.HasMany(c => c.Locations)
            .WithOne(l => l.City)
            .HasForeignKey(l => l.CityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}