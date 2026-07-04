using EliteEvents.Application.DTOs.Booking;
using FluentValidation;

namespace EliteEvents.Application.Validators;

public class BookingCreateValidator : AbstractValidator<BookingCreateDto>
{
    public BookingCreateValidator()
    {
        RuleFor(x => x.EventDate)
            .NotEmpty().WithMessage("Event date is required.")
            .Must(date => date >= DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Event date must be today or in the future.");

        RuleFor(x => x.GuestCount)
            .GreaterThan(0).WithMessage("Guest count must be at least 1.")
            .LessThanOrEqualTo(10000).WithMessage("Guest count cannot exceed 10,000.");

        RuleFor(x => x)
            .Must(x => x.EventId.HasValue || x.VenueId.HasValue || x.PackageId.HasValue)
            .WithMessage("At least one of Event, Venue, or Package must be selected.");

        RuleForEach(x => x.Details).SetValidator(new BookingDetailCreateValidator());
    }
}

public class BookingDetailCreateValidator : AbstractValidator<BookingDetailCreateDto>
{
    public BookingDetailCreateValidator()
    {
        RuleFor(x => x.ServiceName)
            .NotEmpty().WithMessage("Service name is required.")
            .MaximumLength(200);

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be at least 1.");

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Unit price cannot be negative.");
    }
}

public class BookingStatusUpdateValidator : AbstractValidator<BookingStatusUpdateDto>
{
    public BookingStatusUpdateValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(s => new[] { "Pending", "Confirmed", "InProgress", "Completed", "Cancelled", "Refunded" }.Contains(s))
            .WithMessage("Invalid booking status.");

        RuleFor(x => x.CancelReason)
            .NotEmpty()
            .When(x => x.Status == "Cancelled")
            .WithMessage("Cancel reason is required when cancelling a booking.");
    }
}
