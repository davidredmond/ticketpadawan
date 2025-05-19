using FastEndpoints;
using FluentValidation;
using TP.Domain.Models.Venue;

namespace TP.RestfulAPI.Validations
{
    public class VenueCreateModelValidator : Validator<VenueCreateModel>
    {
        public VenueCreateModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Venue name is required.")
                .MaximumLength(100)
                .WithMessage("Venue name must not exceed 100 characters.");
            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required.")
                .MaximumLength(200)
                .WithMessage("Address must not exceed 200 characters.");
            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required.")
                .MaximumLength(100)
                .WithMessage("City must not exceed 100 characters.");
            RuleFor(x => x.Region)
                .NotEmpty()
                .WithMessage("Region is required.")
                .MaximumLength(100)
                .WithMessage("Region must not exceed 100 characters.");
            RuleFor(x => x.Country)
                .NotEmpty()
                .WithMessage("Country is required.")
                .MaximumLength(100)
                .WithMessage("Country must not exceed 100 characters.");
            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .WithMessage("Postal code is required.")
                .MaximumLength(20)
                .WithMessage("Postal code must not exceed 20 characters.");
        }
    }
}
