using EliteEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EliteEvents.Infrastructure.Data.Configurations;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.ToTable("Venues");

        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).UseIdentityColumn();

        builder.Property(v => v.Name).IsRequired().HasMaxLength(200);
        builder.Property(v => v.Description).HasMaxLength(2000);
        builder.Property(v => v.Address).IsRequired().HasMaxLength(500);
        builder.Property(v => v.Latitude).HasColumnType("decimal(10,8)");
        builder.Property(v => v.Longitude).HasColumnType("decimal(11,8)");
        builder.Property(v => v.PricePerHour).HasColumnType("decimal(18,2)");
        builder.Property(v => v.PricePerDay).HasColumnType("decimal(18,2)");
        builder.Property(v => v.ContactPerson).HasMaxLength(100);
        builder.Property(v => v.ContactPhone).HasMaxLength(20);
        builder.Property(v => v.ContactEmail).HasMaxLength(256);
        builder.Property(v => v.Facilities).HasMaxLength(2000);
        builder.Property(v => v.Rules).HasMaxLength(2000);
        builder.Property(v => v.CoverImageUrl).HasMaxLength(500);
        builder.Property(v => v.Rating).HasColumnType("decimal(3,2)");

        builder.HasIndex(v => v.CityId).HasFilter("[IsDeleted] = 0");
        builder.HasIndex(v => v.IsFeatured).HasFilter("[IsDeleted] = 0 AND [IsActive] = 1");

        builder.HasOne(v => v.City)
            .WithMany()
            .HasForeignKey(v => v.CityId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(v => v.Images)
            .WithOne(i => i.Venue)
            .HasForeignKey(i => i.VenueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(v => v.Availability)
            .WithOne(a => a.Venue)
            .HasForeignKey(a => a.VenueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(v => !v.IsDeleted);
    }
}
