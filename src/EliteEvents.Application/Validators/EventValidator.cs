using EliteEvents.Application.DTOs.Event;
using FluentValidation;

namespace EliteEvents.Application.Validators;

public class EventCreateValidator : AbstractValidator<EventCreateDto>
{
    public EventCreateValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Category is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Start date must be today or later.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be on or after start date.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.");

        RuleFor(x => x.DiscountPrice)
            .LessThan(x => x.Price)
            .When(x => x.DiscountPrice.HasValue)
            .WithMessage("Discount price must be less than the regular price.");

        RuleFor(x => x.MaxAttendees)
            .GreaterThan(0)
            .When(x => x.MaxAttendees.HasValue)
            .WithMessage("Max attendees must be greater than 0.");
    }
}

public class EventUpdateValidator : AbstractValidator<EventUpdateDto>
{
    public EventUpdateValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Event ID is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Category is required.");

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be on or after start date.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative.");
    }
}
