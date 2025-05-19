namespace TP.Domain.Commands
{
    public interface ICommand<TResult> { }

    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> HandleAsync(TCommand command);
    }
}
