using EliteEvents.Application.DTOs.Payment;
using FluentValidation;

namespace EliteEvents.Application.Validators;

public class PaymentCreateValidator : AbstractValidator<PaymentCreateDto>
{
    public PaymentCreateValidator()
    {
        RuleFor(x => x.BookingId)
            .GreaterThan(0).WithMessage("Booking ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Payment amount must be greater than 0.");

        RuleFor(x => x.PaymentMethod)
            .NotEmpty().WithMessage("Payment method is required.")
            .Must(m => new[] { "UPI", "CreditCard", "DebitCard", "NetBanking", "Cash", "BankTransfer" }.Contains(m))
            .WithMessage("Invalid payment method.");
    }
}

public class PaymentRefundValidator : AbstractValidator<PaymentRefundDto>
{
    public PaymentRefundValidator()
    {
        RuleFor(x => x.PaymentId)
            .GreaterThan(0).WithMessage("Payment ID is required.");

        RuleFor(x => x.RefundAmount)
            .GreaterThan(0).WithMessage("Refund amount must be greater than 0.");

        RuleFor(x => x.RefundReason)
            .NotEmpty().WithMessage("Refund reason is required.")
            .MaximumLength(500).WithMessage("Refund reason cannot exceed 500 characters.");
    }
}
