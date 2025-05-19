using FastEndpoints;
using TP.Domain;
using TP.Domain.Models.Result;
using TP.Domain.Models.Venue;
using TP.Domain.Queries.Venue;

namespace TP.RestfulAPI.Venues
{
    public class VenueListEndpoint : EndpointWithoutRequest<IEnumerable<VenueModel>>
    {
        private readonly IDispatcher _dispatcher;

        public VenueListEndpoint(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Configure()
        {
            Get("/venues");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var venuesResult = await _dispatcher.SendQueryAsync<GetAllVenuesQuery, WorkResult<IEnumerable<VenueModel>>>(new GetAllVenuesQuery());

            if (venuesResult.IsSuccess)
            {
                await SendResultAsync(TypedResults.Ok(venuesResult));
            }
            else
            {
                await SendResultAsync(TypedResults.BadRequest(venuesResult));
            }
        }
    }
}
