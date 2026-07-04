using EliteEvents.Application.DTOs.Venue;
using FluentValidation;

namespace EliteEvents.Application.Validators;

public class VenueCreateValidator : AbstractValidator<VenueCreateDto>
{
    public VenueCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Venue name is required.")
            .MaximumLength(200).WithMessage("Venue name cannot exceed 200 characters.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0).WithMessage("Capacity must be greater than 0.");

        RuleFor(x => x.PricePerHour)
            .GreaterThanOrEqualTo(0)
            .When(x => x.PricePerHour.HasValue)
            .WithMessage("Price per hour cannot be negative.");

        RuleFor(x => x.PricePerDay)
            .GreaterThanOrEqualTo(0)
            .When(x => x.PricePerDay.HasValue)
            .WithMessage("Price per day cannot be negative.");

        RuleFor(x => x.ContactEmail)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.ContactEmail))
            .WithMessage("Invalid email format.");
    }
}
