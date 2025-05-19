using FastEndpoints;
using FluentValidation;
using TP.Domain.Models.Event;

namespace TP.RestfulAPI.Validations
{
    public class EventUpdateModelValidator : Validator<EventUpdateModel>
    {
        public EventUpdateModelValidator()
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

            RuleFor(x => x.IsCancelled)
                .NotNull()
                .WithMessage("Event cancellation status is required.");

            RuleFor(x => x.IsDeleted)
                .NotNull()
                .WithMessage("Event deletion status is required.");
        }
    }
}
