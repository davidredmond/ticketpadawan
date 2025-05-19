using FastEndpoints;
using TP.Domain;
using TP.Domain.Commands.Event;
using TP.Domain.Models.Event;
using TP.Domain.Models.Result;

namespace TP.RestfulAPI.Events
{
    public class EventUpdateEndpoint : Endpoint<EventUpdateModel>
    {
        private readonly IDispatcher _dispatcher;

        public EventUpdateEndpoint(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Configure()
        {
            Put("/event/{eventId}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(EventUpdateModel eventModel, CancellationToken ct)
        {
            var eventId = Route<int>("eventId");
            var eventUpdateResult = await _dispatcher.SendCommandAsync<UpdateEventCommand, WorkResult<int>>(new UpdateEventCommand(eventId, eventModel));

            if (eventUpdateResult.IsSuccess)
            {
                await SendResultAsync(TypedResults.Ok(eventUpdateResult));
            }
            else
            {
                await SendResultAsync(TypedResults.BadRequest(eventUpdateResult));
            }
        }
    }
}
