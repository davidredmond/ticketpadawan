using FastEndpoints;
using TP.Domain;
using TP.Domain.Models.Event;
using TP.Domain.Models.Result;
using TP.Domain.Queries.Event;

namespace TP.RestfulAPI.Events
{
    public class EventEndpoint : EndpointWithoutRequest<WorkResult<EventModel>>
    {
        private IDispatcher _dispatcher;

        public EventEndpoint(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Configure()
        {
            Get("/event/{eventId}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var eventId = Route<int>("eventId");
            var eventGetResult = await _dispatcher.SendQueryAsync<GetEventByIdQuery, WorkResult<EventModel>>(new GetEventByIdQuery(eventId));

            if (eventGetResult.IsSuccess)
            {
                await SendAsync(eventGetResult);
            }
            else
            {
                await SendResultAsync(TypedResults.BadRequest(eventGetResult));
            }
        }
    }
}
