using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Protos;
using TP.Domain;
using TP.Domain.Models.Event;
using TP.Domain.Models.Result;
using TP.Domain.Queries.Event;

namespace TP.GrpcServer.Services
{
    public class EventService : Protos.EventService.EventServiceBase
    {
        private readonly ILogger<EventService> _logger;
        private readonly IDispatcher _dispatcher;

        public EventService(ILogger<EventService> logger, IDispatcher dispatcher)
        {
            _logger = logger;
            _dispatcher = dispatcher;
        }

        public override async Task<EventResponse> GetEventById(GetEventByIdRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received request for event with ID: {EventId}", request.EventId);
            var eventResponse = await _dispatcher.SendQueryAsync<GetEventByIdQuery, WorkResult<EventModel>>(new GetEventByIdQuery(request.EventId));

            if (eventResponse.IsSuccess)
            {
                return new EventResponse
                {
                    Description = eventResponse.Value!.Description,
                    Id = eventResponse.Value.Id,
                    Name = eventResponse.Value.Name,
                    StartDate = eventResponse.Value.StartTime.ToUniversalTime().ToTimestamp()
                };
            }

            return new EventResponse();
        }

        public override async Task<GetEventsResponse> GetEvents(EmptyRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received request for all active events");
            var eventResponse = await _dispatcher.SendQueryAsync<GetAllActiveEventsQuery, WorkResult<IEnumerable<EventModel>>>(new GetAllActiveEventsQuery());

            if (eventResponse.IsSuccess)
            {
                var response = new GetEventsResponse();
                response.Events.Add(eventResponse.Value!.Select(a => new EventResponse
                {
                    Description = a.Description,
                    Id = a.Id,
                    Name = a.Name,
                    StartDate = a.StartTime.ToUniversalTime().ToTimestamp()
                }));
                return response;
            }

            return new GetEventsResponse();
        }
    }
}
