using FastEndpoints;
using TP.Domain;
using TP.Domain.Models.Event;
using TP.Domain.Models.Result;
using TP.Domain.Queries.Event;

namespace TP.RestfulAPI.Events
{
    public class EventListEndpoint : EndpointWithoutRequest<WorkResult<IEnumerable<EventModel>>>
    {
        private IDispatcher _dispatcher;

        public EventListEndpoint(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Configure()
        {
            Get("/events");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var eventsResult = await _dispatcher.SendQueryAsync<GetAllActiveEventsQuery, WorkResult<IEnumerable<EventModel>>>(new GetAllActiveEventsQuery());

            if (eventsResult.IsSuccess)
            {
                await SendResultAsync(TypedResults.Ok(eventsResult));
            }
            else
            {
                await SendResultAsync(TypedResults.BadRequest(eventsResult));
            }
        }
    }
}
