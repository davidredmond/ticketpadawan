using FastEndpoints;
using TP.Domain;
using TP.Domain.Commands.Ticket;
using TP.Domain.Models.Result;
using TP.Domain.Models.Ticket;

namespace TP.RestfulAPI.Tickets
{
    public class TicketRefundEndpoint : EndpointWithoutRequest<WorkResult<IEnumerable<TicketModel>>>
    {
        private readonly IDispatcher _dispatcher;

        public TicketRefundEndpoint(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Configure()
        {
            Post("/tickets/{eventId}/refund/{ticketId}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var eventId = Route<int>("eventId");
            var ticketId = Route<int>("ticketId");
            var ticketRefundResult = await _dispatcher.SendCommandAsync<RefundTicketCommand, WorkResult<bool>>(new RefundTicketCommand(eventId, ticketId));

            if (ticketRefundResult.IsSuccess)
            {
                await SendResultAsync(TypedResults.Ok(ticketRefundResult));
            }
            else
            {
                await SendResultAsync(TypedResults.BadRequest(ticketRefundResult));
            }
        }
    }
}
