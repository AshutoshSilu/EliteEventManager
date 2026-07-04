using EliteEvents.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EliteEvents.Infrastructure.Data;

/// <summary>
/// Entity Framework Core database context for the Elite Event Management System.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Identity & Access
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Employee> Employees => Set<Employee>();

    // Vendor Management
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<VendorCategory> VendorCategories => Set<VendorCategory>();

    // Venue Management
    public DbSet<Venue> Venues => Set<Venue>();
    public DbSet<VenueImage> VenueImages => Set<VenueImage>();
    public DbSet<VenueAvailability> VenueAvailabilities => Set<VenueAvailability>();

    // Event Management
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventCategory> EventCategories => Set<EventCategory>();
    public DbSet<EventImage> EventImages => Set<EventImage>();
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<PackageService> PackageServices => Set<PackageService>();

    // Booking & Payment
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingDetail> BookingDetails => Set<BookingDetail>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Invoice> Invoices => Set<Invoice>();

    // Reviews & Gallery
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Gallery> Gallery => Set<Gallery>();
    public DbSet<Testimonial> Testimonials => Set<Testimonial>();

    // System
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Setting> Settings => Set<Setting>();
    public DbSet<FAQ> FAQs => Set<FAQ>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();

    // Location
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<State> States => Set<State>();
    public DbSet<City> Cities => Set<City>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    /// <summary>
    /// Override SaveChanges to automatically set audit fields.
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is Domain.Common.AuditableEntity auditable)
            {
                if (entry.State == EntityState.Added)
                    auditable.CreatedAt = DateTime.UtcNow;
                else if (entry.State == EntityState.Modified)
                    auditable.UpdatedAt = DateTime.UtcNow;
            }

            if (entry.Entity is Domain.Common.AuditableEntityGuid auditableGuid)
            {
                if (entry.State == EntityState.Added)
                    auditableGuid.CreatedAt = DateTime.UtcNow;
                else if (entry.State == EntityState.Modified)
                    auditableGuid.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
