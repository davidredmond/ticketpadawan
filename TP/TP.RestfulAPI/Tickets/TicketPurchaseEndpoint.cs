using FastEndpoints;
using TP.Domain;
using TP.Domain.Commands.Ticket;
using TP.Domain.Models.Result;
using TP.Domain.Models.Ticket;

namespace TP.RestfulAPI.Tickets
{
    public class TicketPurchaseEndpoint : EndpointWithoutRequest<WorkResult<IEnumerable<TicketModel>>>
    {
        private readonly IDispatcher _dispatcher;

        public TicketPurchaseEndpoint(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Configure()
        {
            Post("/tickets/{eventId}/{tierId}/{count}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var eventId = Route<int>("eventId");
            var tierId = Route<int>("tierId");
            var count = Route<int>("count");
            var ticketPurchaseResult = await _dispatcher.SendCommandAsync<PurchaseTicketCommand, WorkResult<IEnumerable<TicketModel>>>(new PurchaseTicketCommand(eventId, tierId, count));

            if (ticketPurchaseResult.IsSuccess)
            {
                await SendResultAsync(TypedResults.Ok(ticketPurchaseResult));
            }
            else
            {
                await SendResultAsync(TypedResults.BadRequest(ticketPurchaseResult));
            }
        }
    }
}
