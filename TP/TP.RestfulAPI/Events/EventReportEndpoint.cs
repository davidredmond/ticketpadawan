using FastEndpoints;
using TP.Domain;
using TP.Domain.Models.Event;
using TP.Domain.Models.Result;
using TP.Domain.Queries.Report;

namespace TP.RestfulAPI.Events
{
    public class EventReportEndpoint : EndpointWithoutRequest<WorkResult<EventSalesModel>>
    {
        private readonly IDispatcher _dispatcher;

        public EventReportEndpoint(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Configure()
        {
            Get("/event/{eventId}/report");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var eventId = Route<int>("eventId");
            var eventSalesResult = await _dispatcher.SendQueryAsync<GetSalesReportForEventQuery, WorkResult<EventSalesModel>>(new GetSalesReportForEventQuery(eventId));

            if (eventSalesResult.IsSuccess)
            {
                await SendAsync(eventSalesResult);
            }
            else
            {
                await SendResultAsync(TypedResults.BadRequest(eventSalesResult));
            }
        }
    }
}
