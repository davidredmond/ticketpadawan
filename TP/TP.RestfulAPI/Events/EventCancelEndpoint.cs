using FastEndpoints;
using TP.Domain;
using TP.Domain.Commands.Event;
using TP.Domain.Enum;
using TP.Domain.Models.Result;

namespace TP.RestfulAPI.Events
{
    public class EventCancelEndpoint : EndpointWithoutRequest<WorkResult<int>>
    {
        private readonly IDispatcher _dispatcher;

        public EventCancelEndpoint(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Configure()
        {
            Put("/event/{eventId}/cancel");
            Policies(UserRoleEnum.ADMINISTRATOR.ToFriendlyString());
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var eventId = Route<int>("eventId");
            var eventCancelResult = await _dispatcher.SendCommandAsync<CancelEventCommand, WorkResult<int>>(new CancelEventCommand(eventId));

            if (eventCancelResult.IsSuccess)
            {
                await SendResultAsync(TypedResults.Ok(eventCancelResult));
            }
            else
            {
                await SendResultAsync(TypedResults.BadRequest(eventCancelResult));
            }
        }
    }
}
