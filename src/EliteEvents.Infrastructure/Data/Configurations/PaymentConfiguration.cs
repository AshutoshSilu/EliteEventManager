using EliteEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EliteEvents.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).UseIdentityColumn();

        builder.Property(p => p.PaymentNumber).IsRequired().HasMaxLength(20);
        builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");
        builder.Property(p => p.TransactionId).HasMaxLength(100);
        builder.Property(p => p.GatewayResponse).HasMaxLength(2000);
        builder.Property(p => p.RefundAmount).HasColumnType("decimal(18,2)");
        builder.Property(p => p.RefundReason).HasMaxLength(500);
        builder.Property(p => p.Notes).HasMaxLength(500);
        builder.Property(p => p.PaymentMethod).HasConversion<string>().HasMaxLength(30);
        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(p => p.PaymentNumber).IsUnique();
        builder.HasIndex(p => p.BookingId);
        builder.HasIndex(p => p.CustomerId);
        builder.HasIndex(p => p.Status);

        builder.HasOne(p => p.Booking)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Customer)
            .WithMany()
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id).UseIdentityColumn();

        builder.Property(i => i.InvoiceNumber).IsRequired().HasMaxLength(20);
        builder.Property(i => i.SubTotal).HasColumnType("decimal(18,2)");
        builder.Property(i => i.TaxAmount).HasColumnType("decimal(18,2)");
        builder.Property(i => i.DiscountAmount).HasColumnType("decimal(18,2)");
        builder.Property(i => i.TotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(i => i.PaidAmount).HasColumnType("decimal(18,2)");
        builder.Property(i => i.DueAmount).HasColumnType("decimal(18,2)");
        builder.Property(i => i.Notes).HasMaxLength(500);
        builder.Property(i => i.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasIndex(i => i.InvoiceNumber).IsUnique();

        builder.HasOne(i => i.Booking)
            .WithOne(b => b.Invoice)
            .HasForeignKey<Invoice>(i => i.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Customer)
            .WithMany()
            .HasForeignKey(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
