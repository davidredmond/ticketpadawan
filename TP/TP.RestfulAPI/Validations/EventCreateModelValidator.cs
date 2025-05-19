using FastEndpoints;
using FluentValidation;
using TP.Domain.Models.Event;

namespace TP.RestfulAPI.Validations
{
    public class EventCreateModelValidator : Validator<EventCreateModel>
    {
        public EventCreateModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Event name is required.")
                .MaximumLength(100)
                .WithMessage("Event name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Event description is required.")
                .MaximumLength(500)
                .WithMessage("Event description must not exceed 500 characters.");

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .WithMessage("Event start date is required.")
                .Must(date => date > DateTime.UtcNow)
                .WithMessage("Event start date must be in the future.");

            RuleFor(x => x.EndTime)
                .NotEmpty()
                .WithMessage("Event end date is required.")
                .Must((model, endDate) => endDate > model.StartTime)
                .WithMessage("Event end date must be after the start date.");

            When(x => x.EndTime <= x.StartTime, () =>
            {
                RuleFor(x => x.EndTime)
                    .Must((model, endDate) => endDate > model.StartTime)
                    .WithMessage("Event end date must be after the start date.");
            });

            RuleFor(x => x.VenueId)
                .NotEmpty()
                .WithMessage("Venue ID is required.")
                .GreaterThan(0)
                .WithMessage("Venue ID must match an existing venue.");

            RuleFor(x => x.TicketPrices)
                .NotEmpty()
                .WithMessage("At least one ticket price is required.");
        }
    }
}
