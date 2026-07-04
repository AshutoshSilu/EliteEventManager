using EliteEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EliteEvents.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasDefaultValueSql("NEWSEQUENTIALID()");

        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(500);
        builder.Property(u => u.PhoneNumber).HasMaxLength(20);
        builder.Property(u => u.ProfileImageUrl).HasMaxLength(500);
        builder.Property(u => u.EmailVerificationToken).HasMaxLength(500);
        builder.Property(u => u.PasswordResetToken).HasMaxLength(500);
        builder.Property(u => u.RefreshToken).HasMaxLength(500);

        builder.HasIndex(u => u.Email).IsUnique().HasFilter("[IsDeleted] = 0");
        builder.HasIndex(u => u.RoleId).HasFilter("[IsDeleted] = 0");

        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Customer)
            .WithOne(c => c.User)
            .HasForeignKey<Customer>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Employee)
            .WithOne(e => e.User)
            .HasForeignKey<Employee>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Vendor)
            .WithOne(v => v.User)
            .HasForeignKey<Vendor>(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(u => !u.IsDeleted);

        builder.Ignore(u => u.FullName);
    }
}
