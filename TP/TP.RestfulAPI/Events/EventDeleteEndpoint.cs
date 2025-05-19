using FastEndpoints;
using TP.Domain;
using TP.Domain.Commands.Event;
using TP.Domain.Models.Result;

namespace TP.RestfulAPI.Events
{
    public class EventDeleteEndpoint : EndpointWithoutRequest<WorkResult<int>>
    {
        private readonly IDispatcher _dispatcher;

        public EventDeleteEndpoint(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Configure()
        {
            Delete("/event/{eventId}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var eventId = Route<int>("eventId");
            var eventDeleteResult = await _dispatcher.SendCommandAsync<DeleteEventCommand, WorkResult<int>>(new DeleteEventCommand(eventId));

            if (eventDeleteResult.IsSuccess)
            {
                await SendResultAsync(TypedResults.Ok(eventDeleteResult));
            }
            else
            {
                await SendResultAsync(TypedResults.BadRequest(eventDeleteResult));
            }
        }
    }
}
