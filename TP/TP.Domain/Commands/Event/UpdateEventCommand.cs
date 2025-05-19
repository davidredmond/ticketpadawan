using TP.Database;
using TP.Domain.Models.Event;
using TP.Domain.Models.Result;

namespace TP.Domain.Commands.Event
{
    public record UpdateEventCommand(int Id, EventUpdateModel Model) : ICommand<WorkResult<int>> { }

    public class UpdateEventCommandHandler : ICommandHandler<UpdateEventCommand, WorkResult<int>>
    {
        private readonly TPDbContext _dbContext;
        public UpdateEventCommandHandler(TPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkResult<int>> HandleAsync(UpdateEventCommand command)
        {
            var dbEvent = await _dbContext.Events.FindAsync(command.Id);

            if (dbEvent == null)
            {
                return new WorkResult<int>(command.Id)
                {
                    IsSuccess = false,
                    Message = "Event not found"
                };
            }

            dbEvent.Name = command.Model.Name;
            dbEvent.Updated = DateTime.UtcNow;
            dbEvent.Description = command.Model.Description;
            dbEvent.EndTime = command.Model.EndTime;
            dbEvent.StartTime = command.Model.StartTime;
            dbEvent.IsCancelled = command.Model.IsCancelled;
            dbEvent.IsDeleted = command.Model.IsDeleted;

            try
            {
                await _dbContext.SaveChangesAsync();
                return new WorkResult<int>(command.Id)
                {
                    IsSuccess = true,
                    Message = "Event updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new WorkResult<int>(command.Id)
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
