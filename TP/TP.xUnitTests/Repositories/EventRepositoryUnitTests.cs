using Moq;
using TP.Domain.Models.Venue;
using TP.Domain.Models.Event;
using TP.Domain.Models.Result;
using TP.Domain.Models.Ticket;
using TP.Domain;
using TP.Domain.Queries.Event;
using TP.Domain.Commands.Event;
using TP.Domain.Queries.Report;

namespace TP.xUnitTests.Repositories
{
    public class EventRepositoryUnitTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock;

        public EventRepositoryUnitTests()
        {
            _dispatcherMock = new Mock<IDispatcher>();
        }

        [Fact]
        public async Task GetAll_ReturnsMultipleEvents()
        {
            var eventModels = new List<EventModel>
            {
                CreateEventModel(1),
                CreateEventModel(2)
            };

            _dispatcherMock.Setup(a => a.SendQueryAsync<GetAllActiveEventsQuery, WorkResult<IEnumerable<EventModel>>>(new GetAllActiveEventsQuery()))
                .ReturnsAsync(new WorkResult<IEnumerable<EventModel>>(eventModels)
                {
                    Message = "Success",
                    IsSuccess = true
                });

            var result = await _dispatcherMock.Object.SendQueryAsync<GetAllActiveEventsQuery, WorkResult<IEnumerable<EventModel>>>(new GetAllActiveEventsQuery());
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(eventModels.Count, result.Value.Count());
            Assert.Equal(eventModels.First().Name, result.Value.First().Name);
        }

        [Fact]
        public async Task GetById_ReturnsSingleEvent()
        {
            var eventModel = CreateEventModel(1);

            _dispatcherMock.Setup(a => a.SendQueryAsync<GetEventByIdQuery, WorkResult<EventModel>>(new GetEventByIdQuery(eventModel.Id)))
                .ReturnsAsync(new WorkResult<EventModel>(eventModel)
                {
                    Message = "Success",
                    IsSuccess = true
                });

            var result = await _dispatcherMock.Object.SendQueryAsync<GetEventByIdQuery, WorkResult<EventModel>>(new GetEventByIdQuery(eventModel.Id));
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(eventModel.Name, result.Value.Name);
        }

        [Fact]
        public async Task Create_ReturnsEventModel()
        {
            var eventCreateModel = new EventCreateModel
            {
                Description = "Test Description",
                Name = "Test Event",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(1),
                TicketPrices = new List<TicketCreatePriceModel>
                    {
                        new TicketCreatePriceModel { Price = 100, TierName = "VIP", TotalAvailable = 10 }
                    },
                VenueId = 1
            };
            var eventModel = CreateEventModel(1);

            _dispatcherMock.Setup(a => a.SendCommandAsync<CreateEventCommand, WorkResult<EventModel>>(new CreateEventCommand(eventCreateModel)))
                .ReturnsAsync(new WorkResult<EventModel>(eventModel)
                {
                    Message = "Success",
                    IsSuccess = true
                });

            var result = await _dispatcherMock.Object.SendCommandAsync<CreateEventCommand, WorkResult<EventModel>>(new CreateEventCommand(eventCreateModel));
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(eventModel.Name, result.Value.Name);
        }

        [Fact]
        public async Task Delete_ReturnsInt()
        {
            var eventModel = CreateEventModel(1);

            _dispatcherMock.Setup(a => a.SendCommandAsync<DeleteEventCommand, WorkResult<int>>(new DeleteEventCommand(eventModel.Id)))
                .ReturnsAsync(new WorkResult<int>(eventModel.Id)
                {
                    Message = "Success",
                    IsSuccess = true
                });

            var result = await _dispatcherMock.Object.SendCommandAsync<DeleteEventCommand, WorkResult<int>>(new DeleteEventCommand(eventModel.Id));
            Assert.True(result.IsSuccess);
            Assert.Equal(eventModel.Id, result.Value);
        }

        [Fact]
        public async Task Update_ReturnsInt()
        {
            var eventModel = CreateEventModel(1);
            var eventUpdateModel = new EventUpdateModel
            {
                Description = "Updated Description",
                Name = "Updated Event",
                StartTime = DateTime.Now.AddDays(1),
                EndTime = DateTime.Now.AddDays(2),
                IsCancelled = false,
                IsDeleted = false
            };

            _dispatcherMock.Setup(a => a.SendCommandAsync<UpdateEventCommand, WorkResult<int>>(new UpdateEventCommand(eventModel.Id, eventUpdateModel)))
                .ReturnsAsync(new WorkResult<int>(eventModel.Id)
                {
                    Message = "Success",
                    IsSuccess = true
                });

            var result = await _dispatcherMock.Object.SendCommandAsync<UpdateEventCommand, WorkResult<int>>(new UpdateEventCommand(eventModel.Id, eventUpdateModel));
            Assert.True(result.IsSuccess);
            Assert.Equal(eventModel.Id, result.Value);
        }

        [Fact]
        public async Task Cancel_ReturnsInt()
        {
            var eventModel = CreateEventModel(1);

            _dispatcherMock.Setup(a => a.SendCommandAsync<CancelEventCommand, WorkResult<int>>(new CancelEventCommand(eventModel.Id)))
                .ReturnsAsync(new WorkResult<int>(eventModel.Id)
                {
                    Message = "Success",
                    IsSuccess = true
                });

            var result = await _dispatcherMock.Object.SendCommandAsync<CancelEventCommand, WorkResult<int>>(new CancelEventCommand(eventModel.Id));
            Assert.True(result.IsSuccess);
            Assert.Equal(eventModel.Id, result.Value);
        }

        [Fact]
        public async Task SalesReport_ReturnsSalesModel()
        {
            var eventModel = CreateEventModel(1);
            var eventSalesModel = new EventSalesModel
            {
                Event = eventModel,
                TotalRevenue = 1000.00M,
                TotalTickets = 1000,
                TotalTicketsRemaining = 500,
                TotalTicketsSold = 500,
                TicketPrices = new List<TicketSalesModel>
                {
                    new TicketSalesModel
                    {
                        Price = 100,
                        TierId = 1,
                        TierName = "VIP",
                        Revenue = 10000.00M,
                        TotalTickets = 1000,
                        TotalTicketsRemaining = 500,
                        TotalTicketsSold = 500
                    }
                }
            };

            _dispatcherMock.Setup(a => a.SendQueryAsync<GetSalesReportForEventQuery, WorkResult<EventSalesModel>>(new GetSalesReportForEventQuery(eventModel.Id)))
                .ReturnsAsync(new WorkResult<EventSalesModel>(eventSalesModel)
                {
                    Message = "Success",
                    IsSuccess = true
                });

            var result = await _dispatcherMock.Object.SendQueryAsync<GetSalesReportForEventQuery, WorkResult<EventSalesModel>>(new GetSalesReportForEventQuery(eventModel.Id));
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(eventSalesModel.Event.Id, result.Value.Event.Id);
            Assert.Equal(eventSalesModel.TotalRevenue, result.Value.TotalRevenue);
            Assert.Equal(eventSalesModel.TicketPrices.Count(), result.Value.TicketPrices.Count());
        }

        #region MoqObjects
        private EventModel CreateEventModel(int id) => new EventModel
        {
            Id = id,
            Name = "Test Event",
            Description = "Test Description",
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddDays(1),
            TicketPrices = new List<TicketPriceModel>
                {
                    new TicketPriceModel
                    {
                        Price = 100,
                        TierId = 1,
                        TierName = "VIP",
                        TotalAvailable = 10
                    }
                },
            Venue = new VenueModel
            {
                Id = 1,
                Name = "Test Venue",
                Address = "Test Address",
                City = "Test City",
                Country = "Test Country",
                PostalCode = "12345",
                Region = "Test Region"
            }
        };
        #endregion
    }
}
