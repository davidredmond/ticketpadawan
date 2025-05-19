using Moq;
using TP.Domain;
using TP.Domain.Commands.Venue;
using TP.Domain.Models.Result;
using TP.Domain.Models.Venue;
using TP.Domain.Queries.Venue;

namespace TP.xUnitTests.Repositories
{
        public class VenueUnitTests
        {
            private readonly Mock<IDispatcher> _dispatcherMock;

            public VenueUnitTests()
            {
                _dispatcherMock = new Mock<IDispatcher>();
            }

            [Fact]
            public async Task CreateAsync_ReturnsVenueModel()
            {
                var venueCreateModel = new VenueCreateModel
                {
                    Name = "Test Venue",
                    Address = "Test Address",
                    City = "Test City",
                    Country = "Test Country",
                    PostalCode = "12345",
                    Region = "Test Region"
                };
                var createVenueCommand = new CreateVenueCommand(venueCreateModel);
                _dispatcherMock.Setup(a => a.SendCommandAsync<CreateVenueCommand, WorkResult<VenueModel>>(createVenueCommand))
                    .ReturnsAsync(new WorkResult<VenueModel>(CreateVenueModel(1))
                    {
                        Message = "Success",
                        IsSuccess = true
                    });
                var result = await _dispatcherMock.Object.SendCommandAsync<CreateVenueCommand, WorkResult<VenueModel>>(createVenueCommand);
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Value);
                Assert.Equal(venueCreateModel.Name, result.Value.Name);
            }

            [Fact]
            public async Task GetAllAsync_ReturnsMultipleVenues()
            {
                var venueModels = new List<VenueModel>()
                {
                    CreateVenueModel(1),
                    CreateVenueModel(2)
                };

                _dispatcherMock.Setup(a => a.SendQueryAsync<GetAllVenuesQuery, WorkResult<IEnumerable<VenueModel>>>(new GetAllVenuesQuery()))
                    .ReturnsAsync(new WorkResult<IEnumerable<VenueModel>>(venueModels)
                    {
                        Message = "Success",
                        IsSuccess = true
                    });

                var result = await _dispatcherMock.Object.SendQueryAsync<GetAllVenuesQuery, WorkResult<IEnumerable<VenueModel>>>(new GetAllVenuesQuery());
                Assert.True(result.IsSuccess);
                Assert.NotNull(result.Value);
                Assert.Equal(venueModels.Count(), result.Value.Count());
                Assert.Equal(venueModels.First().Name, result.Value.First().Name);
            }

            #region MoqObjects
            private VenueModel CreateVenueModel(int id)
            {
                return new VenueModel
                {
                    Id = id,
                    Name = "Test Venue",
                    Address = "Test Address",
                    City = "Test City",
                    Country = "Test Country",
                    PostalCode = "12345",
                    Region = "Test Region"
                };
            }
            #endregion
    }
}
