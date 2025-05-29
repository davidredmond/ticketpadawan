using FastEndpoints;
using TP.Domain;
using TP.Domain.Enum;
using TP.Domain.Models.Result;
using TP.Domain.Models.Ticket;
using TP.Domain.Queries.Ticket;

namespace TP.RestfulAPI.Tickets
{
    public class TicketAvailabilityEndpoint : EndpointWithoutRequest<WorkResult<IEnumerable<TicketPriceModel>>>
    {
        private readonly IDispatcher _dispatcher;

        public TicketAvailabilityEndpoint(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Configure()
        {
            Get("/tickets/{eventId}");
            Policies(UserRoleEnum.USER.ToFriendlyString());
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var eventId = Route<int>("eventId");
            var ticketsAvailableResult = await _dispatcher.SendQueryAsync<GetAvailableTicketsForEventQuery, WorkResult<IEnumerable<TicketPriceModel>>>(new GetAvailableTicketsForEventQuery(eventId));
            
            if (ticketsAvailableResult.IsSuccess)
            {
                await SendResultAsync(TypedResults.Ok(ticketsAvailableResult));
            }
            else
            {
                await SendResultAsync(TypedResults.BadRequest(ticketsAvailableResult));
            }
        }
    }
}
