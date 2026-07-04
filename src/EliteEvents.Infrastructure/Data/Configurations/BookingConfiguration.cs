using EliteEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EliteEvents.Infrastructure.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).UseIdentityColumn();

        builder.Property(b => b.BookingNumber).IsRequired().HasMaxLength(20);
        builder.Property(b => b.SpecialRequests).HasMaxLength(2000);
        builder.Property(b => b.SubTotal).HasColumnType("decimal(18,2)");
        builder.Property(b => b.DiscountAmount).HasColumnType("decimal(18,2)");
        builder.Property(b => b.TaxAmount).HasColumnType("decimal(18,2)");
        builder.Property(b => b.TotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(b => b.Notes).HasMaxLength(1000);
        builder.Property(b => b.CancelReason).HasMaxLength(500);
        builder.Property(b => b.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(b => b.BookingNumber).IsUnique();
        builder.HasIndex(b => b.CustomerId);
        builder.HasIndex(b => b.Status);
        builder.HasIndex(b => b.EventDate);

        builder.HasOne(b => b.Customer)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Event)
            .WithMany(e => e.Bookings)
            .HasForeignKey(b => b.EventId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Venue)
            .WithMany(v => v.Bookings)
            .HasForeignKey(b => b.VenueId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Package)
            .WithMany(p => p.Bookings)
            .HasForeignKey(b => b.PackageId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.Approver)
            .WithMany()
            .HasForeignKey(b => b.ApprovedBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(b => b.Details)
            .WithOne(d => d.Booking)
            .HasForeignKey(d => d.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.Payments)
            .WithOne(p => p.Booking)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Invoice)
            .WithOne(i => i.Booking)
            .HasForeignKey<Invoice>(i => i.BookingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
