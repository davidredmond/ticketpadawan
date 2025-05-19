using Moq;
using TP.Domain;
using TP.Domain.Commands.Ticket;
using TP.Domain.Models.Result;
using TP.Domain.Models.Ticket;
using TP.Domain.Queries.Ticket;

namespace TP.xUnitTests.Repositories
{
    public class TicketRepositoryUnitTests
    {
        private readonly Mock<IDispatcher> _dispatcherMock;

        public TicketRepositoryUnitTests()
        {
            _dispatcherMock = new Mock<IDispatcher>();
        }

        [Fact]
        public async Task PurchaseAsync_ReturnsMultipleTickets()
        {
            var ticketModels = new List<TicketModel>
            {
                new TicketModel { Id = 1, SoldTime = DateTime.UtcNow },
                new TicketModel { Id = 2, SoldTime = DateTime.UtcNow }
            };
            const int eventId = 1;
            const int tierId = 1;
            var purchaseTicketCommand = new PurchaseTicketCommand(eventId, tierId, ticketModels.Count);

            _dispatcherMock.Setup(a => a.SendCommandAsync<PurchaseTicketCommand, WorkResult<IEnumerable<TicketModel>>>(purchaseTicketCommand))
                .ReturnsAsync(new WorkResult<IEnumerable<TicketModel>>(ticketModels)
                {
                    Message = "Success",
                    IsSuccess = true
                });

            var result = await _dispatcherMock.Object.SendCommandAsync<PurchaseTicketCommand, WorkResult<IEnumerable<TicketModel>>>(purchaseTicketCommand);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(ticketModels.Count(), result.Value.Count());
        }

        [Fact]
        public async Task AvailableAsync_ReturnsMultipleTicketPrices()
        {
            var ticketPriceModels = new List<TicketPriceModel>
            {
                new TicketPriceModel { TierId = 1, Price = 100, TierName = "VIP", TotalAvailable = 20 },
                new TicketPriceModel { TierId = 2, Price = 50, TierName = "Standard", TotalAvailable = 100 }
            };

            _dispatcherMock.Setup(a => a.SendQueryAsync<GetAvailableTicketsForEventQuery, WorkResult<IEnumerable<TicketPriceModel>>>(new GetAvailableTicketsForEventQuery(1)))
                .ReturnsAsync(new WorkResult<IEnumerable<TicketPriceModel>>(ticketPriceModels)
                {
                    Message = "Success",
                    IsSuccess = true
                });

            var result = await _dispatcherMock.Object.SendQueryAsync<GetAvailableTicketsForEventQuery, WorkResult<IEnumerable<TicketPriceModel>>>(new GetAvailableTicketsForEventQuery(1));
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(ticketPriceModels.Count(), result.Value.Count());
            Assert.Equal(ticketPriceModels.Sum(a => a.TotalAvailable), result.Value.Sum(a => a.TotalAvailable));
            Assert.Equal(ticketPriceModels.First().TierName, result.Value.First().TierName);
        }
    }
}
