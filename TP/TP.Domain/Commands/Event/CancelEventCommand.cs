using TP.Database;
using TP.Domain.Models.Result;

namespace TP.Domain.Commands.Event
{
    public record CancelEventCommand(int Id) : ICommand<WorkResult<int>> { }

    public class CancelEventCommandHandler : ICommandHandler<CancelEventCommand, WorkResult<int>>
    {
        private readonly TPDbContext _dbContext;

        public CancelEventCommandHandler(TPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WorkResult<int>> HandleAsync(CancelEventCommand command)
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

            dbEvent.Updated = DateTime.UtcNow;
            dbEvent.IsCancelled = true;

            try
            {
                await _dbContext.SaveChangesAsync();
                return new WorkResult<int>(command.Id)
                {
                    IsSuccess = true,
                    Message = "Event cancelled successfully"
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
