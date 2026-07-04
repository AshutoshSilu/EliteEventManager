using EliteEvents.Application.Services.Interfaces;
using EliteEvents.Domain.Interfaces;
using EliteEvents.Infrastructure.Data;
using EliteEvents.Infrastructure.Identity;
using EliteEvents.Infrastructure.Repositories;
using EliteEvents.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EliteEvents.Infrastructure;

/// <summary>
/// Registers Infrastructure layer services with the DI container.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Database Context
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
                }));

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IRepositoryGuid<>), typeof(RepositoryGuid<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IVenueRepository, VenueRepository>();
        services.AddScoped<IVendorRepository, VendorRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Identity Services
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        // External Services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileStorageService, FileStorageService>();

        return services;
    }
}
