using EliteEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EliteEvents.Infrastructure.Data.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityColumn();

        builder.Property(e => e.Title).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Description).HasMaxLength(4000);
        builder.Property(e => e.Price).HasColumnType("decimal(18,2)");
        builder.Property(e => e.DiscountPrice).HasColumnType("decimal(18,2)");
        builder.Property(e => e.CoverImageUrl).HasMaxLength(500);
        builder.Property(e => e.Tags).HasMaxLength(500);
        builder.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(e => e.CategoryId).HasFilter("[IsDeleted] = 0");
        builder.HasIndex(e => e.VenueId).HasFilter("[IsDeleted] = 0");
        builder.HasIndex(e => e.Status).HasFilter("[IsDeleted] = 0");
        builder.HasIndex(e => e.StartDate).HasFilter("[IsDeleted] = 0");

        builder.HasOne(e => e.Category)
            .WithMany(c => c.Events)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Venue)
            .WithMany(v => v.Events)
            .HasForeignKey(e => e.VenueId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Organizer)
            .WithMany()
            .HasForeignKey(e => e.OrganizerId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(e => e.Images)
            .WithOne(i => i.Event)
            .HasForeignKey(i => i.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.Ignore(e => e.AvailableSeats);
    }
}
