using EliteEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EliteEvents.Infrastructure.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).UseIdentityColumn();
        builder.Property(r => r.Name).IsRequired().HasMaxLength(50);
        builder.Property(r => r.Description).HasMaxLength(200);

        builder.HasIndex(r => r.Name).IsUnique();
    }
}

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("RolePermissions");

        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        builder.HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).UseIdentityColumn();
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Module).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Description).HasMaxLength(200);

        builder.HasIndex(p => p.Name).IsUnique();
    }
}
