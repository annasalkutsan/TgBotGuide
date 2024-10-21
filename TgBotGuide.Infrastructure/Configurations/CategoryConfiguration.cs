using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TgBotGuide.Domain.Entities;

namespace TgBotGuide.Infrastructure.Configurations;

public class CategoryConfiguration: IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id");

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("name");

        builder.Property(c => c.CreationDate)
            .HasColumnName("creation_date");

        // связь с LocationCategory (1:N)
        builder.HasMany(c => c.LocationsCategories)
            .WithOne(lc => lc.Category)
            .HasForeignKey(lc => lc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}