using FastEndpoints;
using TP.Domain;
using TP.Domain.Commands.Event;
using TP.Domain.Enum;
using TP.Domain.Models.Event;
using TP.Domain.Models.Result;

namespace TP.RestfulAPI.Events
{
    public class EventCreateEndpoint : Endpoint<EventCreateModel, WorkResult<EventModel>>
    {
        private readonly IDispatcher _dispatcher;

        public EventCreateEndpoint(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Configure()
        {
            Post("/event");
            Policies(UserRoleEnum.ADMINISTRATOR.ToFriendlyString());
        }

        public override async Task HandleAsync(EventCreateModel eventCreateModel, CancellationToken ct)
        {
            var eventCreateResult = await _dispatcher.SendCommandAsync<CreateEventCommand, WorkResult<EventModel>>(new CreateEventCommand(eventCreateModel));

            if (eventCreateResult.IsSuccess)
            {
                await SendResultAsync(TypedResults.Ok(eventCreateResult));
            }
            else
            {
                await SendResultAsync(TypedResults.BadRequest(eventCreateResult));
            }
        }
    }
}
