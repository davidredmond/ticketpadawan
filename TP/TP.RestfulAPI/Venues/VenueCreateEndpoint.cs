using FastEndpoints;
using TP.Domain;
using TP.Domain.Commands.Venue;
using TP.Domain.Models.Result;
using TP.Domain.Models.Venue;

namespace TP.RestfulAPI.Venues
{
    public class VenueCreateEndpoint : Endpoint<VenueCreateModel, WorkResult<VenueModel>>
    {
        private readonly IDispatcher _dispatcher;

        public VenueCreateEndpoint(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Configure()
        {
            Post("/venue");
            AllowAnonymous();
        }

        public override async Task HandleAsync(VenueCreateModel venueCreateModel, CancellationToken ct)
        {
            var venueCreateCommand = new CreateVenueCommand(venueCreateModel);
            var venueCreateResult = await _dispatcher.SendCommandAsync<CreateVenueCommand, WorkResult<VenueModel>>(venueCreateCommand);

            if (venueCreateResult.IsSuccess)
            {
                await SendResultAsync(TypedResults.Ok(venueCreateResult));
            }
            else
            {
                await SendResultAsync(TypedResults.BadRequest(venueCreateResult));
            }
        }
    }
}
