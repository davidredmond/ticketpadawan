using TP.Database;
using TP.Database.Models;
using TP.Domain.Models;
using TP.Domain.Models.Result;
using TP.Domain.Models.Venue;

namespace TP.Domain.Commands.Venue
{
    public class CreateVenueCommand : ICommand<WorkResult<VenueModel>>
    { 
        public VenueCreateModel Model { get; set; }

        public CreateVenueCommand(VenueCreateModel model)
        {
            Model = model;
        }
    }

    public class CreateVenueCommandHandler : ICommandHandler<CreateVenueCommand, WorkResult<VenueModel>>
    {
        private readonly TPDbContext _dbContext;

        public CreateVenueCommandHandler(TPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkResult<VenueModel>> HandleAsync(CreateVenueCommand command)
        {
            var dbVenue = await _dbContext.Venues.AddAsync(new Database.Models.Venue
            {
                Created = DateTime.UtcNow,
                Location = new Location
                {
                    Address = command.Model.Address,
                    City = command.Model.City,
                    Country = command.Model.Country,
                    Created = DateTime.UtcNow,
                    PostalCode = command.Model.PostalCode,
                    Region = command.Model.Region,
                    Updated = DateTime.UtcNow
                },
                Name = command.Model.Name,
                Updated = DateTime.UtcNow
            });

            try
            {
                await _dbContext.SaveChangesAsync();
                return new WorkResult<VenueModel>(MappingHelper.MapVenueToVenueModel(dbVenue.Entity))
                {
                    IsSuccess = true,
                    Message = "Venue created successfully"
                };
            }
            catch (Exception ex)
            {
                return new WorkResult<VenueModel>(null)
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
