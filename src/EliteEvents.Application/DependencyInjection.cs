using EliteEvents.Application.Mappings;
using EliteEvents.Application.Services.Implementations;
using EliteEvents.Application.Services.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EliteEvents.Application;

/// <summary>
/// Registers Application layer services with the DI container.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Application services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IVenueService, VenueService>();
        services.AddScoped<IVendorService, VendorService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IReportService, ReportService>();

        return services;
    }
}
