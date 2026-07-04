using EliteEvents.Application.DTOs.Review;
using FluentValidation;

namespace EliteEvents.Application.Validators;

public class ReviewCreateValidator : AbstractValidator<ReviewCreateDto>
{
    public ReviewCreateValidator()
    {
        RuleFor(x => x.EntityType)
            .NotEmpty().WithMessage("Entity type is required.")
            .Must(t => new[] { "Event", "Venue", "Vendor", "Package" }.Contains(t))
            .WithMessage("Invalid entity type.");

        RuleFor(x => x.EntityId)
            .GreaterThan(0).WithMessage("Entity ID is required.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.Comment)
            .MaximumLength(2000).WithMessage("Comment cannot exceed 2000 characters.");
    }
}

public class ReviewReplyValidator : AbstractValidator<ReviewReplyDto>
{
    public ReviewReplyValidator()
    {
        RuleFor(x => x.ReviewId)
            .GreaterThan(0).WithMessage("Review ID is required.");

        RuleFor(x => x.Reply)
            .NotEmpty().WithMessage("Reply is required.")
            .MaximumLength(1000).WithMessage("Reply cannot exceed 1000 characters.");
    }
}
