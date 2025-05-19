using FastEndpoints;
using FluentValidation;
using TP.Domain.Models.Ticket;

namespace TP.RestfulAPI.Validations
{
    public class TicketCreatePriceModelValidation : Validator<TicketCreatePriceModel>
    {
        public TicketCreatePriceModelValidation()
        {
            RuleFor(x => x.TierName)
                .NotEmpty()
                .WithMessage("Tier name is required.")
                .MaximumLength(100)
                .WithMessage("Tier name must not exceed 100 characters.");

            RuleFor(x => x.Price)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Ticket prices must be greater than zero.");

            RuleFor(x => x.TotalAvailable)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("Tickets available must be greater tan zero.");
        }
    }
}
